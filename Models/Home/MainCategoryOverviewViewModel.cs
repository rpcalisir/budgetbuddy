using BudgetBuddy.Domain.Enums;

namespace BudgetBuddy.Web.Models.Home
{
    public class MainCategoryOverviewViewModel
    {
        public Guid Id { get; set; }  // Added this
        public MainCategoryType Type { get; set; }
        public string TypeName => Type.ToString();
        public decimal Percentage { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SubCategoryOverviewViewModel> SubCategories { get; set; } = new();
    }
}
