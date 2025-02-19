namespace BudgetBuddy.Web.Models.Domain
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public int MainCategoryId { get; set; }
    }
}
