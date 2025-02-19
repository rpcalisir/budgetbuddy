using BudgetBuddy.Web.Models.Domain;

namespace BudgetBuddy.Web.ViewModels.Home
{
    public class BudgetOverviewViewModel
    {
        public decimal Salary { get; set; }
        public int Year { get; set; }
        public List<MainCategory> MainCategories { get; set; }
    }

}
