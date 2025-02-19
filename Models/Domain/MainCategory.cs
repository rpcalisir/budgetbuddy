using BudgetBuddy.Web.Models.Enums;

namespace BudgetBuddy.Web.Models.Domain
{
    public class MainCategory
    {
        public int Id { get; set; }
        public MainCategoryType Type { get; set; }
        public decimal Percentage { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SubCategory> SubCategories { get; set; } = new();
        public string TypeName => Type == MainCategoryType.Custom ? CustomName : Type.ToString();
        public string CustomName { get; set; }
    }
}
