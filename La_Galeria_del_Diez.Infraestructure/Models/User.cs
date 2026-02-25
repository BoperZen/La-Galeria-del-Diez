using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public int IdRol { get; set; }

    public bool? UserState { get; set; }

    public virtual ICollection<Auction> Auction { get; set; } = new List<Auction>();

    public virtual ICollection<AuctionableObject> AuctionableObject { get; set; } = new List<AuctionableObject>();

    public virtual ICollection<Bidding> Bidding { get; set; } = new List<Bidding>();

    public virtual Rol IdRolNavigation { get; set; } = null!;
}
