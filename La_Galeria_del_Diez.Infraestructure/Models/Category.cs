using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<AuctionableObject> IdObject { get; set; } = new List<AuctionableObject>();
}
