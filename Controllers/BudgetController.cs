using BudgetBuddy.Domain.Entities;
using BudgetBuddy.Web.Models;
using BudgetBuddy.Web.Models.Budget;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Web.Controllers
{
    public class BudgetController : Controller
    {
        private readonly Budget _budget; // This will be replaced with proper DI later

        public BudgetController()
        {
            // Temporary: Create a new budget for testing
            _budget = new Budget(100000, DateTime.Now.Year);
        }

        public IActionResult Index()
        {
            if (_budget == null)
            {
                return View("Error", new ErrorViewModel { Message = "Budget is null" });
            }

            // Create and populate the view model
            var viewModel = new BudgetViewModel
            {
                Id = _budget.Id,
                Salary = _budget.Salary,
                Year = _budget.Year,
                MainCategories = _budget.MainCategories.Select(mc => new MainCategoryViewModel
                {
                    Id = mc.Id,
                    Type = mc.Type,
                    Percentage = mc.Percentage,
                    TotalAmount = mc.TotalAmount,
                    SubCategories = mc.SubCategories.Select(sc => new SubCategoryViewModel
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        Amount = sc.Amount,
                        Percentage = sc.Percentage
                    }).ToList() ?? new List<SubCategoryViewModel>()
                }).ToList() ?? new List<MainCategoryViewModel>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateMainCategory(Guid id, decimal amount)
        {
            var mainCategory = _budget.MainCategories.First(mc => mc.Id == id);
            mainCategory.SetTotalAmount(amount, _budget.Salary);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddSubCategory(Guid mainCategoryId, string name)
        {
            var mainCategory = _budget.MainCategories.First(mc => mc.Id == mainCategoryId);
            mainCategory.AddSubCategory(name);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateSubCategory(Guid mainCategoryId, string subCategoryName, decimal amount)
        {
            var mainCategory = _budget.MainCategories.First(mc => mc.Id == mainCategoryId);
            mainCategory.UpdateSubCategoryAmount(subCategoryName, amount, _budget.Salary); 

            return RedirectToAction(nameof(Index));
        }
    }
}
