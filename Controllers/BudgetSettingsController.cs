using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Web.Controllers
{
    public class BudgetSettingsController : Controller
    {
        private readonly IBudgetService _budgetService;

        public BudgetSettingsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
