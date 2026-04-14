using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;

namespace La_Galeria_del_Diez.Application.Services.Implementatios
{
    public class ServiceBidding : IServiceBidding
    {
        private readonly IRepositoryBidding _repositoryBidding;
        private readonly IRepositoryAuction _repositoryAuction;
        private readonly IMapper _mapper;

        public ServiceBidding(IRepositoryBidding repositoryBidding, IRepositoryAuction repositoryAuction, IMapper mapper)
        {
            _repositoryBidding = repositoryBidding;
            _repositoryAuction = repositoryAuction;
            _mapper = mapper;
        }

        public async Task AddAsync(BiddingDTO dto)
        {
            var auction = await _repositoryAuction.FindByIdAsync(dto.IdAuction);
            if (auction == null)
            {
                throw new InvalidOperationException("La subasta no existe.");
            }

            var now = DateTime.Now;
            var isActive = auction.IdState == 2 && auction.StartDate <= now && auction.EndDate > now;
            if (!isActive)
            {
                throw new InvalidOperationException("La subasta no está activa.");
            }

            if (auction.IdUser == dto.IdUser)
            {
                throw new InvalidOperationException("El vendedor no puede pujar en su propia subasta.");
            }

            var currentPrice = auction.Bidding.Any() ? auction.Bidding.Max(b => b.Amount) : auction.BasePrice;
            if (dto.Amount <= currentPrice)
            {
                throw new InvalidOperationException("El monto debe ser mayor a la puja actual.");
            }

            var minRequired = currentPrice + auction.MinIncrement;
            if (dto.Amount < minRequired)
            {
                throw new InvalidOperationException($"El monto debe ser al menos {minRequired:N0}.");
            }

            var bidding = _mapper.Map<Bidding>(dto);
            bidding.RegistrationDate = DateTime.Now;

            await _repositoryBidding.AddAsync(bidding);
        }
    }
}
