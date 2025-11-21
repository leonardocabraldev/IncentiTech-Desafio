using Application.Repositories;
using Domain.LavaCar.Entitites;
using Shared.DTOs.Common;

namespace Application.Services.Servico
{
    public class GetServicesByUser
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServicesByUser(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public PagedResult<Service> Execute(string responsibleUser, int page, int pageSize)
        {
            return _serviceRepository.GetByUser(responsibleUser, page, pageSize);
        }
    }
}