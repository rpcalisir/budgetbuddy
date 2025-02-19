using BudgetBuddy.Web.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class CreateMainCategoryViewModel
{
    [Required(ErrorMessage = "Please select a category type")]
    public MainCategoryType? Type { get; set; }  // Make it nullable

    public string? CustomName { get; set; }

    [Required(ErrorMessage = "Percentage is required")]
    [Range(0.1, 100, ErrorMessage = "Percentage must be between 0.1 and 100")]
    public decimal Percentage { get; set; }
}