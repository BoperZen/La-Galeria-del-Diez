namespace La_Galeria_del_Diez.Web.Models
{
    public class BiddingCreateViewModel
    {
        public int AuctionId { get; set; }

        public string AuctionName { get; set; } = string.Empty;

        public decimal CurrentPrice { get; set; }

        public decimal MinIncrement { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string BuyerName { get; set; } = string.Empty;

        public string BuyerEmail { get; set; } = string.Empty;
    }
}
