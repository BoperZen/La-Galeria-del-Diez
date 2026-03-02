using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.DTOs
{
    public record AuctionDTO
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal BasePrice { get; set; }

        public decimal MinIncrement { get; set; }

        public int IdState { get; set; }

        public int IdUser { get; set; }

        public int IdObject { get; set; }

        public int? AutionWinner { get; set; }

        public List<BiddingDTO> Biddings { get; set; } = new List<BiddingDTO>();

        public UserDTO IdUserNavegation { get; set; } = new UserDTO();

        public UserDTO? Winner { get; set; }

        public StateDTO State { get; set; } = new StateDTO();
    }
}
