using BudgetBuddy.Web.Models.Domain;

namespace BudgetBuddy.Web.ViewModels.BudgetSettings
{
    public class BudgetSettingsViewModel
    {
        public List<MainCategory> MainCategories { get; set; } = new();
        public decimal TotalBudgetPercentage { get; set; }
        public decimal RemainingPercentage => 100 - TotalBudgetPercentage;
        public const decimal TOTAL_SALARY = 100000;
    }
}
