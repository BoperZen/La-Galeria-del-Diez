namespace La_Galeria_del_Diez.Application.DTOs.Reports
{
    public class BidHistoryReportDto
    {
        public string AuctionTitle { get; set; } = string.Empty;
        public string ObjectName { get; set; } = string.Empty;
        public string Bidder { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string BidStatus { get; set; } = string.Empty;
    }
}
