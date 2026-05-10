
namespace EGYmotor.Models
{
    public class MonthlyProfit
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalAmount { get; set; }
        public int Transactions { get; set; }
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
    }
}