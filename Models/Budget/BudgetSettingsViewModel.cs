namespace BudgetBuddy.Web.Models.Budget
{
    public class BudgetSettingsViewModel
    {
        public decimal Salary { get; set; }
        public int Year { get; set; }
        public List<MainCategorySettingsViewModel> MainCategories { get; set; }
    }
}
