using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class Image
{
    public int Id { get; set; }

    public byte[] Data { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public int IdObject { get; set; }

    public virtual AuctionableObject IdObjectNavigation { get; set; } = null!;
}
