using BudgetBuddy.Web.Models.Domain;
using BudgetBuddy.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Web.Controllers
{
    public class HomeController : Controller
    {
        private static List<MainCategory> _mainCategories = new();
        private const decimal DEFAULT_SALARY = 100000;

        public IActionResult Index()
        {
            var viewModel = new BudgetOverviewViewModel
            {
                Salary = DEFAULT_SALARY,
                Year = DateTime.Now.Year,
                MainCategories = _mainCategories
            };

            return View(viewModel);
        }
    }
}
