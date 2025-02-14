using BudgetBuddy.Domain.Base;
using BudgetBuddy.Domain.Exceptions;

namespace BudgetBuddy.Domain.Entities
{
    public class SubCategory : BaseEntity
    {
        public string Name { get; private set; }
        public decimal Percentage { get; private set; }
        public decimal Amount { get; private set; }

        private SubCategory() : base() { }

        public SubCategory(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Sub-category name cannot be empty");

            Name = name;
            Percentage = 0;
            Amount = 0;
        }

        // Set amount and calculate percentage
        public void SetAmount(decimal amount, decimal budgetTotalAmount)
        {
            if (amount < 0)
                throw new DomainException("Amount cannot be negative");

            Amount = amount;
            // Calculate percentage based on budget total amount
            Percentage = budgetTotalAmount > 0
                ? Math.Round((Amount / budgetTotalAmount) * 100, 2)
                : 0;

            ModifiedDate = DateTime.UtcNow;
        }
    }
}
