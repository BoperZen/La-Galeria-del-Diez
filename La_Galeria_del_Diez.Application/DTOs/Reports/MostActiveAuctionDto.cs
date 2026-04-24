namespace La_Galeria_del_Diez.Application.DTOs.Reports
{
    public class MostActiveAuctionDto
    {
        public int Rank { get; set; }
        public string AuctionTitle { get; set; } = string.Empty;
        public string ObjectName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TotalBids { get; set; }
        public decimal HighestBid { get; set; }
        public string LastBidder { get; set; } = string.Empty;
    }
}
