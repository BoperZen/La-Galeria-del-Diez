using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class AuctionableObject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Condition { get; set; }

    public DateTime RegistrationDate { get; set; }

    public int IdState { get; set; }

    public int IdUser { get; set; }

    public virtual ICollection<Auction> Auction { get; set; } = new List<Auction>();

    public virtual State IdStateNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Image> Image { get; set; } = new List<Image>();

    public virtual ICollection<Category> IdCategory { get; set; } = new List<Category>();
}
