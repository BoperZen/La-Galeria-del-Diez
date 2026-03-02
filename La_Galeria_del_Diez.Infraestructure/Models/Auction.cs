using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class Auction
{
    public int Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal BasePrice { get; set; }

    public decimal MinIncrement { get; set; }

    public int IdState { get; set; }

    public int IdUser { get; set; }

    public int IdObject { get; set; }

    public int? AuctionWinner { get; set; }

    public virtual ICollection<Bidding> Bidding { get; set; } = new List<Bidding>();

    public virtual AuctionableObject IdObjectNavigation { get; set; } = null!;

    public virtual State IdStateNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual User Winner { get; set; } = null!;
}
