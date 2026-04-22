namespace EGYmotor.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalFeedbacks { get; set; }
        public int TotalOrders { get; set; }
        public int TotalPayments { get; set; }

        public List<RegisterUser> Users { get; set; } 
    }
}
