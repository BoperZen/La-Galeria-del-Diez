namespace La_Galeria_del_Diez.Application.DTOs.Reports
{
    public class TopValuedItemDto
    {
        public int Rank { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int AuctionCount { get; set; }
        public string CurrentStatus { get; set; } = string.Empty;
    }
}
