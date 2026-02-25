using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class Bidding
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime RegistrationDate { get; set; }

    public int IdUser { get; set; }

    public int IdAuction { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public virtual Auction IdAuctionNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
