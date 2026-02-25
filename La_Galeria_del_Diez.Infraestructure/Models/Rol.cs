using System;
using System.Collections.Generic;

namespace La_Galeria_del_Diez.Infraestructure.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<User> User { get; set; } = new List<User>();
}
