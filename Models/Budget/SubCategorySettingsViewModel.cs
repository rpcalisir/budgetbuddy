namespace BudgetBuddy.Web.Models.Budget
{
    public class SubCategorySettingsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public Dictionary<string, MonthlyBudget> MonthlyBudgets { get; set; }
    }
}
