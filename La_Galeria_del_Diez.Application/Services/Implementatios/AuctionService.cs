using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace La_Galeria_del_Diez.Application.Services.Implementatios
{
    public class ServiceAuction : IServiceAuction
    {
        private readonly IRepositoryAuction _repository;
        private readonly IMapper _mapper;

        public ServiceAuction(IRepositoryAuction repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AuctionDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<AuctionDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<AuctionDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<AuctionDTO>>(list);
        }

        public async Task<ICollection<AuctionDTO>> ListActiveAsync(DateTime now)
        {
            var list = await _repository.ListActiveAsync(now);
            return _mapper.Map<ICollection<AuctionDTO>>(list);
        }

        public async Task<ICollection<AuctionDTO>> ListFinishedAsync(DateTime now)
        {
            var list = await _repository.ListFinishedAsync(now);
            return _mapper.Map<ICollection<AuctionDTO>>(list);
        }

        public async Task<ICollection<AuctionDTO>> ListDraftAsync(DateTime now)
        {
            var list = await _repository.ListDraftAsync(now);
            return _mapper.Map<ICollection<AuctionDTO>>(list);
        }

        public async Task<ICollection<AuctionDTO>> ListPendingPaymentByWinnerAsync(int winnerUserId)
        {
            var list = await _repository.ListPendingPaymentAsync();
            var auctions = _mapper.Map<ICollection<AuctionDTO>>(list);

            return auctions
                .Where(a =>
                {
                    var leadingBid = a.Biddings?
                        .OrderByDescending(b => b.Amount)
                        .ThenByDescending(b => b.RegistrationDate)
                        .FirstOrDefault();

                    var resolvedWinner = a.AutionWinner ?? leadingBid?.IdUser;
                    return resolvedWinner == winnerUserId;
                })
                .OrderByDescending(a => a.EndDate)
                .ToList();
        }

        public async Task<ICollection<int>> CloseExpiredAuctionsAsync(DateTime now)
        {
            return await _repository.CloseExpiredAuctionsAsync(now);
        }

        public async Task AddAsync(AuctionDTO dto)
        {
            var auction = _mapper.Map<Auction>(dto);
            await _repository.AddAsync(auction);
        }

        public async Task UpdateAsync(int id, AuctionDTO dto)
        {
            // Traer entity (idealmente trackeado) antes de mapear encima
            var entity = await _repository.FindByIdAsync(id);

            // Map "sobre" el entity existente (mantiene tracking)
            _mapper.Map(dto, entity);

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);
            await _repository.DeleteAsync(entity);
        }

        public async Task UpdateStateAsync(int id, int stateId)
        {
            await _repository.UpdateStateAsync(id, stateId);
        }
    }
}