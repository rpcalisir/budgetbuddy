using BudgetBuddy.Domain.Base;
using BudgetBuddy.Domain.Enums;
using BudgetBuddy.Domain.Exceptions;

namespace BudgetBuddy.Domain.Entities
{
    public class Budget : BaseEntity
    {
        public decimal Salary { get; private set; }
        private readonly List<MainCategory> _mainCategories;
        public IReadOnlyCollection<MainCategory> MainCategories => _mainCategories.AsReadOnly();
        public int Year { get; private set; }

        private Budget() : base()
        {
            _mainCategories = new List<MainCategory>();
        }

        public Budget(decimal salary, int year) : this()
        {
            if (salary <= 0)
                throw new DomainException("Salary must be greater than zero");

            Salary = salary;
            Year = year;

            // Initialize main categories
            InitializeMainCategories();
        }

        private void InitializeMainCategories()
        {
            // Create default main categories
            foreach (MainCategoryType type in Enum.GetValues(typeof(MainCategoryType)))
            {
                _mainCategories.Add(new MainCategory(type)); // Initial percentage set to 0
            }
        }

        public void UpdateSalary(decimal newSalary)
        {
            if (newSalary <= 0)
                throw new DomainException("Salary must be greater than zero");

            Salary = newSalary;
            ModifiedDate = DateTime.UtcNow;
        }

        public void ValidateTotalPercentage()
        {
            decimal totalPercentage = _mainCategories.Sum(mc => mc.Percentage);
            if (totalPercentage > 100)
                throw new DomainException("Total percentage cannot exceed 100%");
        }
    }
}
