using BudgetBuddy.Domain.Base;
using BudgetBuddy.Domain.Entities;
using BudgetBuddy.Domain.Enums;
using BudgetBuddy.Domain.Exceptions;

public class MainCategory : BaseEntity
{
    public MainCategoryType Type { get; private set; }
    public decimal Percentage { get; private set; }
    public decimal TotalAmount { get; private set; }
    private readonly List<SubCategory> _subCategories;
    public IReadOnlyCollection<SubCategory> SubCategories => _subCategories.AsReadOnly();

    private MainCategory() : base()
    {
        _subCategories = new List<SubCategory>();
    }

    public MainCategory(MainCategoryType type) : this()
    {
        Type = type;
        Percentage = 0;
        TotalAmount = 0;
    }

    // Method to set amount directly and calculate percentage
    public void SetTotalAmount(decimal amount, decimal budgetSalary)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative");
        if (budgetSalary <= 0)
            throw new DomainException("Salary must be greater than zero");

        TotalAmount = amount;
        Percentage = Math.Round((TotalAmount / budgetSalary) * 100, 2);
        ModifiedDate = DateTime.UtcNow;
    }

    // Method to set percentage and calculate amount
    public void SetPercentage(decimal percentage, decimal budgetSalary)
    {
        if (percentage < 0 || percentage > 100)
            throw new DomainException("Percentage must be between 0 and 100");
        if (budgetSalary <= 0)
            throw new DomainException("Salary must be greater than zero");

        Percentage = percentage;
        TotalAmount = budgetSalary * (Percentage / 100);
        ModifiedDate = DateTime.UtcNow;
    }

    public void AddSubCategory(string name)
    {
        if (_subCategories.Any(sc => sc.Name == name))
            throw new DomainException($"Sub-category with name '{name}' already exists");

        var subCategory = new SubCategory(name);
        _subCategories.Add(subCategory);
    }

    public void UpdateSubCategoryAmount(string subCategoryName, decimal amount, decimal budgetTotalAmount)
    {
        var subCategory = _subCategories.FirstOrDefault(sc => sc.Name == subCategoryName)
            ?? throw new DomainException($"Sub-category '{subCategoryName}' not found");

        // Calculate new total including this update
        decimal totalOtherSubCategories = _subCategories
            .Where(sc => sc.Name != subCategoryName)
            .Sum(sc => sc.Amount);

        if (totalOtherSubCategories + amount > TotalAmount)
            throw new DomainException($"Total sub-category amounts cannot exceed main category amount of {TotalAmount}");

        subCategory.SetAmount(amount, budgetTotalAmount);
    }

    public decimal GetRemainingAmount()
    {
        return TotalAmount - _subCategories.Sum(sc => sc.Amount);
    }
}