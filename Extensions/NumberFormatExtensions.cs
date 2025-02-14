namespace BudgetBuddy.Web.Extensions
{
    public static class NumberFormatExtensions
    {
        public static string ToTurkishFormat(this decimal number)
        {
            return string.Format(new System.Globalization.CultureInfo("tr-TR"), "{0:N0}", number);
        }
    }
}