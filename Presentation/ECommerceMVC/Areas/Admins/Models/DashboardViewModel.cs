namespace ECommerceMVC.Areas.Admins.Models
{
    public class DashboardViewModel
    {
        public int filter { get; set; }
        public long CustomerCount { get; set; }
        public long OrderCount { get; set; }
        public long PackageCount { get; set; }
        public long CourierCount { get; set; }
        public decimal CourierPrice { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal? UsedBalance { get; set; }
        public List<OrderPriceTotal> OrderPriceTotals { get; set; }
        public List<PackagePriceTotal> PackagePriceTotals { get; set; }

    }

    public class OrderPriceTotal
    {
        public decimal OrderPrice { get; set; }
        public string Currency { get; set; }
    }
    public class PackagePriceTotal
    {
        public decimal PackagePrice { get; set; }
        public string Currency { get; set; }
    }
}
