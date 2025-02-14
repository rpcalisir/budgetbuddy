namespace BudgetBuddy.Web.Models.Budget
{
    public class BudgetViewModel
    {
        public Guid Id { get; set; }
        public decimal Salary { get; set; }
        public int Year { get; set; }
        public List<MainCategoryViewModel> MainCategories { get; set; } = new();
    }
}
