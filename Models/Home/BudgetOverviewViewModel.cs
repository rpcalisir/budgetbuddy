namespace BudgetBuddy.Web.Models.Home
{
    public class BudgetOverviewViewModel
    {
        public decimal Salary { get; set; }
        public int Year { get; set; }
        public List<MainCategoryOverviewViewModel> MainCategories { get; set; } = new();
    }
}
