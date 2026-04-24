using System;
using System.Collections.Generic;
using La_Galeria_del_Diez.Application.DTOs;

namespace La_Galeria_del_Diez.Application.DTOs.Api
{
    public record AuctionApiDto
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

        public List<BiddingDTO> Biddings { get; set; } = new();

        public UserDTO IdUserNavegation { get; set; } = new();

        public UserDTO? Winner { get; set; }

        public StateDTO State { get; set; } = new();

        public Auctionable_ObjectDTO Object { get; set; } = new();

        public int Count { get; set; }
    }
}
