using BudgetBuddy.Web.Extensions;
using BudgetBuddy.Web.Models.Domain;
using BudgetBuddy.Web.Models.Enums;
using BudgetBuddy.Web.ViewModels.BudgetSettings;
using Microsoft.AspNetCore.Mvc;

public class BudgetSettingsController : Controller
{
    private static List<MainCategory> _mainCategories = new();
    private const decimal TOTAL_SALARY = 100000; // We'll make this configurable later

    public IActionResult Index()
    {
        var viewModel = new BudgetSettingsViewModel
        {
            MainCategories = _mainCategories,
            TotalBudgetPercentage = _mainCategories.Sum(x => x.Percentage)
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult CreateMainCategory(CreateMainCategoryViewModel model)
    {
        // Check if Type has a value
        if (!model.Type.HasValue)
        {
            TempData["Error"] = "Please select a category type";
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            // Check for duplicate main category
            if (_mainCategories.Any(x =>
                (x.Type == model.Type && model.Type != MainCategoryType.Custom) ||
                (model.Type == MainCategoryType.Custom &&
                 x.CustomName?.Equals(model.CustomName, StringComparison.OrdinalIgnoreCase) == true)))
            {
                TempData["Error"] = "This main category already exists";
                return RedirectToAction(nameof(Index));
            }

            // Validate custom name if type is Custom
            if (model.Type == MainCategoryType.Custom)
            {
                if (string.IsNullOrWhiteSpace(model.CustomName))
                {
                    TempData["Error"] = "Custom category name is required";
                    return RedirectToAction(nameof(Index));
                }
            }

            var totalCurrentPercentage = _mainCategories.Sum(x => x.Percentage);
            if (totalCurrentPercentage + model.Percentage > 100)
            {
                TempData["Error"] = "Total budget allocation cannot exceed 100%";
                return RedirectToAction(nameof(Index));
            }

            var mainCategory = new MainCategory
            {
                Id = _mainCategories.Count + 1,
                Type = model.Type.Value, // Use .Value since we know it has a value
                CustomName = model.Type == MainCategoryType.Custom ? model.CustomName : null,
                Percentage = model.Percentage,
                TotalAmount = (model.Percentage / 100) * TOTAL_SALARY,
                SubCategories = new List<SubCategory>()
            };

            _mainCategories.Add(mainCategory);
            TempData["Success"] = "Main category added successfully";
            return RedirectToAction(nameof(Index));
        }

        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        TempData["Error"] = string.Join(", ", errors);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult CreateSubCategory(CreateSubCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var mainCategory = _mainCategories.FirstOrDefault(x => x.Id == model.MainCategoryId);
            if (mainCategory != null)
            {
                // Check for zero or negative amount
                if (model.Amount <= 0)
                {
                    TempData["Error"] = "Amount must be greater than zero";
                    return RedirectToAction(nameof(Index));
                }

                // Check for duplicate sub category name
                if (mainCategory.SubCategories.Any(x => x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    TempData["Error"] = "A sub-category with this name already exists";
                    return RedirectToAction(nameof(Index));
                }

                // Calculate maximum allowed amount for this main category
                decimal maxAllowedAmount = (mainCategory.Percentage / 100) * TOTAL_SALARY;
                decimal currentUsedAmount = mainCategory.SubCategories.Sum(x => x.Amount);
                decimal remainingAmount = maxAllowedAmount - currentUsedAmount;

                // Check if there's any remaining amount
                if (remainingAmount <= 0)
                {
                    TempData["Error"] = "This main category has reached its maximum allocation limit";
                    return RedirectToAction(nameof(Index));
                }

                // Check if amount exceeds the remaining allocation
                if (model.Amount > remainingAmount)
                {
                    TempData["Error"] = $"Cannot add this sub-category. Maximum remaining amount is {remainingAmount.ToTurkishFormat()} ₺";
                    return RedirectToAction(nameof(Index));
                }

                var subCategory = new SubCategory
                {
                    Id = mainCategory.SubCategories.Count + 1,
                    Name = model.Name,
                    Amount = model.Amount,
                    MainCategoryId = model.MainCategoryId,
                    Percentage = Math.Round((model.Amount / TOTAL_SALARY) * 100, 2)
                };

                mainCategory.SubCategories.Add(subCategory);
                mainCategory.TotalAmount = mainCategory.SubCategories.Sum(x => x.Amount);

                TempData["Success"] = "Sub-category added successfully";
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult DeleteMainCategory(int id)
    {
        var category = _mainCategories.FirstOrDefault(x => x.Id == id);
        if (category != null)
        {
            _mainCategories.Remove(category);
            TempData["Success"] = $"{category.TypeName} category deleted successfully";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditMainCategory(int id, decimal percentage)
    {
        var mainCategory = _mainCategories.FirstOrDefault(x => x.Id == id);
        if (mainCategory == null)
        {
            return Json(new { success = false, error = "Category not found" });
        }

        // Check for zero or negative percentage
        if (percentage <= 0)
        {
            return Json(new { success = false, error = "Percentage must be greater than zero" });
        }

        // Check if percentage is greater than 100
        if (percentage > 100)
        {
            return Json(new { success = false, error = "Percentage cannot exceed 100%" });
        }

        // Calculate total percentage excluding current category
        var totalOtherPercentages = _mainCategories
            .Where(x => x.Id != id)
            .Sum(x => x.Percentage);

        // Check if new total would exceed 100%
        if (totalOtherPercentages + percentage > 100)
        {
            var availablePercentage = 100 - totalOtherPercentages;
            return Json(new
            {
                success = false,
                error = $"Total budget allocation cannot exceed 100%. Maximum available percentage is {availablePercentage}%"
            });
        }

        // Calculate new total amount
        decimal newTotalAmount = (percentage / 100) * TOTAL_SALARY;

        // Check if new amount would be less than current sub-categories total
        decimal currentSubCategoriesTotal = mainCategory.SubCategories.Sum(x => x.Amount);
        if (newTotalAmount < currentSubCategoriesTotal)
        {
            return Json(new
            {
                success = false,
                error = $"Cannot reduce allocation below current sub-categories total ({currentSubCategoriesTotal.ToTurkishFormat()} ₺)"
            });
        }

        // All validations passed, update the category
        mainCategory.Percentage = percentage;
        mainCategory.TotalAmount = newTotalAmount;

        // Recalculate sub-category percentages
        foreach (var sub in mainCategory.SubCategories)
        {
            sub.Percentage = Math.Round((sub.Amount / TOTAL_SALARY) * 100, 2);
        }

        return Json(new
        {
            success = true,
            message = $"Main category updated successfully. New allocation: {percentage}% ({newTotalAmount.ToTurkishFormat()} ₺)"
        });
    }

    [HttpPost]
    public IActionResult DeleteSubCategory(int mainCategoryId, int subCategoryId)
    {
        var mainCategory = _mainCategories.FirstOrDefault(x => x.Id == mainCategoryId);
        if (mainCategory != null)
        {
            var subCategory = mainCategory.SubCategories.FirstOrDefault(x => x.Id == subCategoryId);
            if (subCategory != null)
            {
                mainCategory.SubCategories.Remove(subCategory);
                mainCategory.TotalAmount = mainCategory.SubCategories.Sum(x => x.Amount);
                TempData["Success"] = "Sub-category deleted successfully";
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditSubCategory(int mainCategoryId, int subCategoryId, string name, decimal amount)
    {
        var mainCategory = _mainCategories.FirstOrDefault(x => x.Id == mainCategoryId);
        if (mainCategory != null)
        {
            var subCategory = mainCategory.SubCategories.FirstOrDefault(x => x.Id == subCategoryId);
            if (subCategory != null)
            {
                // Check for zero or negative amount
                if (amount <= 0)
                {
                    return Json(new { success = false, error = "Amount must be greater than zero" });
                }

                // Calculate maximum allowed amount for this main category
                decimal maxAllowedAmount = (mainCategory.Percentage / 100) * TOTAL_SALARY;

                // Calculate current total amount excluding the current subcategory
                decimal currentTotalAmount = mainCategory.SubCategories
                    .Where(x => x.Id != subCategoryId)
                    .Sum(x => x.Amount);

                // Calculate remaining amount available
                decimal remainingAmount = maxAllowedAmount - currentTotalAmount;

                // Check if new amount would exceed the limit
                if (amount > remainingAmount)
                {
                    return Json(new { success = false, error = $"Cannot update sub-category. Maximum allowed amount is {remainingAmount.ToTurkishFormat()} ₺" });
                }

                // Check for duplicate name
                if (mainCategory.SubCategories.Any(x => x.Id != subCategoryId &&
                                                      x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    return Json(new { success = false, error = "A sub-category with this name already exists" });
                }

                // Update the subcategory
                subCategory.Name = name;
                subCategory.Amount = amount;
                subCategory.Percentage = Math.Round((amount / TOTAL_SALARY) * 100, 2);

                // Update main category total
                mainCategory.TotalAmount = mainCategory.SubCategories.Sum(x => x.Amount);

                return Json(new { success = true, message = "Sub-category updated successfully" });
            }
        }
        return Json(new { success = false, error = "Category not found" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StoreMessage(string type, string message)
    {
        if (type == "Error")
        {
            TempData["Error"] = message;
        }
        else if (type == "Success")
        {
            TempData["Success"] = message;
        }
        return Json(new { success = true });
    }
}