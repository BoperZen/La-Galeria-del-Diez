using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.DTOs.Api;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Application.Services.Interfaces.Api;

namespace La_Galeria_del_Diez.Application.Services.Implementatios.Api
{
    public class AuctionApiService : IAuctionApiService
    {
        private readonly IServiceAuction _serviceAuction;
        private readonly IMapper _mapper;

        public AuctionApiService(IServiceAuction serviceAuction, IMapper mapper)
        {
            _serviceAuction = serviceAuction;
            _mapper = mapper;
        }

        public async Task<ICollection<AuctionApiDto>> ListAsync()
        {
            var list = await _serviceAuction.ListAsync();
            return _mapper.Map<ICollection<AuctionApiDto>>(list);
        }

        public async Task<AuctionApiDto?> FindByIdAsync(int id)
        {
            var auction = await _serviceAuction.FindByIdAsync(id);
            return _mapper.Map<AuctionApiDto>(auction);
        }

        public async Task AddAsync(AuctionApiDto dto)
        {
            var mapped = _mapper.Map<AuctionDTO>(dto);
            await _serviceAuction.AddAsync(mapped);
        }

        public async Task UpdateAsync(int id, AuctionApiDto dto)
        {
            var mapped = _mapper.Map<AuctionDTO>(dto);
            await _serviceAuction.UpdateAsync(id, mapped);
        }

        public async Task DeleteAsync(int id)
        {
            await _serviceAuction.DeleteAsync(id);
        }
    }
}
