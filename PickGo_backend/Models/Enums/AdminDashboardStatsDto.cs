namespace PickGo_backend.Models.Enums
{
    public class AdminDashboardStatsDto
    {
        public int TotalOrders { get; set; }
        public int ActiveCouriers { get; set; }
        public float TotalRevenue { get; set; }
        public int FailedDeliveries { get; set; }
    }
}
