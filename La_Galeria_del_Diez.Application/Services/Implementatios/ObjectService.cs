using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;

namespace La_Galeria_del_Diez.Application.Services.Implementatios
{
    public class ServiceObject : IServiceObject
    {
        private readonly IRepositoryObject _repository;
        private readonly IMapper _mapper;

        public ServiceObject(IRepositoryObject repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Auctionable_ObjectDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<Auctionable_ObjectDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<Auctionable_ObjectDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<Auctionable_ObjectDTO>>(list);
        }

        public async Task<ICollection<Auctionable_ObjectDTO>> ListAsyncNoAuction()
        {
            var list = await _repository.ListAsyncNoAuction();
            return _mapper.Map<ICollection<Auctionable_ObjectDTO>>(list);
        }

        public async Task AddAsync(Auctionable_ObjectDTO dto)
        {
            ValidateCommon(dto);

            if (!HasValidImages(dto.Images))
            {
                throw new ArgumentException("Debe agregar al menos una imagen.");
            }

            if (dto.RegistrationDate == default)
            {
                dto.RegistrationDate = DateTime.Now;
            }

            var entity = _mapper.Map<AuctionableObject>(dto);
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(Auctionable_ObjectDTO dto)
        {
            if (dto.Id <= 0)
            {
                throw new ArgumentException("Identificador de objeto inválido.");
            }

            ValidateCommon(dto);

            var canEdit = await _repository.CanEditAsync(dto.Id);
            if (!canEdit)
            {
                throw new InvalidOperationException("El objeto no puede editarse porque está en subasta activa o su estado no lo permite.");
            }

            var existing = await _repository.FindByIdAsync(dto.Id);
            var hasExistingImages = existing?.Image?.Any(i => i.Data != null && i.Data.Length > 0) == true;
            var hasNewImages = HasValidImages(dto.Images);

            if (!hasExistingImages && !hasNewImages)
            {
                throw new ArgumentException("Debe agregar al menos una imagen.");
            }

            var entity = _mapper.Map<AuctionableObject>(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Identificador de objeto inválido.");
            }

            await _repository.DeleteAsync(id);
        }

        public Task<bool> CanEditAsync(int id)
        {
            return _repository.CanEditAsync(id);
        }

        public Task<bool> HasActiveAuctionAsync(int id)
        {
            return _repository.HasActiveAuctionAsync(id);
        }

        private static void ValidateCommon(Auctionable_ObjectDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("El nombre es requerido.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description) || dto.Description.Trim().Length < 20)
            {
                throw new ArgumentException("La descripción debe tener al menos 20 caracteres.");
            }

            if (dto.Categories == null || !dto.Categories.Any())
            {
                throw new ArgumentException("Debe seleccionar al menos una categoría.");
            }
        }

        private static bool HasValidImages(List<ImageDTO>? images)
        {
            return images != null && images.Any(i => i.Data != null && i.Data.Length > 0);
        }
    }
}
