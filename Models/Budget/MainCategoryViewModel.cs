using BudgetBuddy.Domain.Enums;

namespace BudgetBuddy.Web.Models.Budget
{
    public class MainCategoryViewModel
    {
        public Guid Id { get; set; }
        public MainCategoryType Type { get; set; }
        public string TypeName => Type.ToString();
        public decimal Percentage { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SubCategoryViewModel> SubCategories { get; set; } = new();
    }
}
