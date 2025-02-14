using BudgetBuddy.Domain.Entities;
using BudgetBuddy.Domain.Enums;
using BudgetBuddy.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var budget = new Budget(100000, DateTime.Now.Year);

            // Get the main categories
            var fixedExpenses = budget.MainCategories.First(mc => mc.Type == MainCategoryType.FixedExpenses);
            var variableExpenses = budget.MainCategories.First(mc => mc.Type == MainCategoryType.VariableExpenses);
            var savings = budget.MainCategories.First(mc => mc.Type == MainCategoryType.Savings);
            var investments = budget.MainCategories.First(mc => mc.Type == MainCategoryType.Investments);

            // Set up Fixed Expenses - percentages will be calculated automatically
            fixedExpenses.SetTotalAmount(35000, budget.Salary);
            fixedExpenses.AddSubCategory("Annem");
            fixedExpenses.UpdateSubCategoryAmount("Annem", 4000, budget.Salary);
            fixedExpenses.AddSubCategory("Berin");
            fixedExpenses.UpdateSubCategoryAmount("Berin", 2000, budget.Salary);
            fixedExpenses.AddSubCategory("E Blok");
            fixedExpenses.UpdateSubCategoryAmount("E Blok", 2000, budget.Salary);
            fixedExpenses.AddSubCategory("Et");
            fixedExpenses.UpdateSubCategoryAmount("Et", 9000, budget.Salary);
            fixedExpenses.AddSubCategory("Market");
            fixedExpenses.UpdateSubCategoryAmount("Market", 10000, budget.Salary);
            fixedExpenses.AddSubCategory("Rel");
            fixedExpenses.UpdateSubCategoryAmount("Rel", 4000, budget.Salary);
            fixedExpenses.AddSubCategory("Su");
            fixedExpenses.UpdateSubCategoryAmount("Su", 500, budget.Salary);
            fixedExpenses.AddSubCategory("Elektrik");
            fixedExpenses.UpdateSubCategoryAmount("Elektrik", 500, budget.Salary);
            fixedExpenses.AddSubCategory("Internet");
            fixedExpenses.UpdateSubCategoryAmount("Internet", 500, budget.Salary);
            fixedExpenses.AddSubCategory("Doğalgaz");
            fixedExpenses.UpdateSubCategoryAmount("Doğalgaz", 2000, budget.Salary);
            fixedExpenses.AddSubCategory("Aidat");
            fixedExpenses.UpdateSubCategoryAmount("Aidat", 500, budget.Salary);

            // Set up Variable Expenses
            variableExpenses.SetTotalAmount(10000, budget.Salary);
            variableExpenses.AddSubCategory("Sosyal Aktiviteler");
            variableExpenses.UpdateSubCategoryAmount("Sosyal Aktiviteler", 4000, budget.Salary);
            variableExpenses.AddSubCategory("Kişisel");
            variableExpenses.UpdateSubCategoryAmount("Kişisel", 4000, budget.Salary);
            variableExpenses.AddSubCategory("Hepsiburada");
            variableExpenses.UpdateSubCategoryAmount("Hepsiburada", 2000, budget.Salary);

            // Set up Savings
            savings.SetTotalAmount(35000, budget.Salary); 
            savings.AddSubCategory("Altın");
            savings.UpdateSubCategoryAmount("Altın", 25000, budget.Salary);
            savings.AddSubCategory("KIS, KUB, DBH, TPZ Fonu,BinanceTR");
            savings.UpdateSubCategoryAmount("KIS, KUB, DBH, TPZ Fonu,BinanceTR", 10000, budget.Salary);

            // Set up Investments
            investments.SetTotalAmount(20000, budget.Salary);
            investments.AddSubCategory("Hisse Fonu");
            investments.UpdateSubCategoryAmount("Hisse Fonu", 20000, budget.Salary);

            // Create view model
            var viewModel = new BudgetOverviewViewModel
            {
                Salary = budget.Salary,
                Year = budget.Year,
                MainCategories = budget.MainCategories.Select(mc => new MainCategoryOverviewViewModel
                {
                    Id = mc.Id,
                    Type = mc.Type,
                    Percentage = mc.Percentage,
                    TotalAmount = mc.TotalAmount,
                    SubCategories = mc.SubCategories.Select(sc => new SubCategoryOverviewViewModel
                    {
                        Name = sc.Name,
                        Amount = sc.Amount,
                        Percentage = sc.Percentage
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        private MainCategoryOverviewViewModel CreateMainCategoryViewModel(MainCategory category)
        {
            return new MainCategoryOverviewViewModel
            {
                Id = category.Id,
                Type = category.Type,
                Percentage = category.Percentage,
                TotalAmount = category.TotalAmount,
                SubCategories = category.SubCategories.Select(sc => new SubCategoryOverviewViewModel
                {
                    Name = sc.Name,
                    Amount = sc.Amount,
                    Percentage = sc.Percentage
                }).ToList()
            };
        }
    }
}
