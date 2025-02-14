using BudgetBuddy.Domain.Enums;

namespace BudgetBuddy.Web.Models.Budget
{
    public class MainCategorySettingsViewModel
    {
        public Guid Id { get; set; }
        public MainCategoryType Type { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
        public List<SubCategorySettingsViewModel> SubCategories { get; set; }
    }
}
