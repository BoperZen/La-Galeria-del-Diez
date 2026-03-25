using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;

namespace La_Galeria_del_Diez.Application.Services.Implementatios
{
    public class ServiceUser : IServiceUser
    {
        private readonly IRepositoryUser _repository;
        private readonly IMapper _mapper;

        public ServiceUser(IRepositoryUser repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<UserDTO>(@object);
            objectMapped.Tally = await Tally(id);
            return objectMapped;
        }

        public async Task<ICollection<UserDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<UserDTO>>(list);
        }

        public async Task UpdateAsync(UserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            await _repository.UpdateAsync(user);
        }

        private async Task<int> Tally(int id)
        {
            return await _repository.Tally(id);
        }
    }
}
