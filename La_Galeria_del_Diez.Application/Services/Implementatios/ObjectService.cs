using AutoMapper;
using La_Galeria_del_Diez.Application.DTOs;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
