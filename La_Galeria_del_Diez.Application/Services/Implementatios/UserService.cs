using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Models;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Libreria.Application.Config;
using Libreria.Application.Utils;
using Microsoft.Extensions.Options;

namespace La_Galeria_del_Diez.Application.Services.Implementatios
{
    public class ServiceUser : IServiceUser
    {
        private readonly IRepositoryUser _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _options;

        public ServiceUser(IRepositoryUser repository, IMapper mapper, IOptions<AppConfig> options)
        {
            _repository = repository;
            _mapper = mapper;
            _options = options;
        }

        public async Task<UserDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<UserDTO>(@object);
            objectMapped.Tally = await Tally(id);
            return objectMapped;
        }

        public async Task<UserDTO> LoginAsync(string id, string password)
        {
            UserDTO usuarioDTO = null!;

            // Llave secreta
            string secret = _options.Value.Crypto.Secret;
            // Password encriptado
            string passwordEncrypted = Cryptography.Encrypt(password, secret);

            var @object = await _repository.LoginAsync(id, passwordEncrypted);

            if (@object != null)
            {
                usuarioDTO = _mapper.Map<UsuarioDTO>(@object);
            }

            return usuarioDTO;
        }

        public async Task<ICollection<UserDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<UserDTO>>(list);
        }

        public async Task<UserDTO> AddAsync(UserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            var created = await _repository.AddAsync(user);
            return _mapper.Map<UserDTO>(created);
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
