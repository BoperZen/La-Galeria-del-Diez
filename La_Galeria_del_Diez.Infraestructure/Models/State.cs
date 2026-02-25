using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class State
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Auction> Auction { get; set; } = new List<Auction>();

    public virtual ICollection<AuctionableObject> AuctionableObject { get; set; } = new List<AuctionableObject>();
}
