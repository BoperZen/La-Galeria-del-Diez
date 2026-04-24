namespace La_Galeria_del_Diez.Application.DTOs.Reports
{
    public class AuctionSummaryReportDto
    {
        public string AuctionTitle { get; set; } = string.Empty;
        public string ObjectName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public int TotalBids { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
