using BudgetBuddy.Domain.Enums;

namespace BudgetBuddy.Web.Models.Budget
{
    public class CategoryViewModel
    {
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public List<MonthlyBudget> MonthlyBudgets { get; set; }
    }
}
