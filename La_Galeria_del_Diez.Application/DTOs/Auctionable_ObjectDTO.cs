using La_Galeria_del_Diez.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.DTOs
{
    public record Auctionable_ObjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Condition { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int IdState { get; set; }
        public int IdUser { get; set; }
        public List<AuctionDTO> Auctions { get; set; } = new List<AuctionDTO>();
        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
        public StateDTO State { get; set; } = new StateDTO();
    }
}
