using Application.Repositories;
using Domain.LavaCar.Entitites;
using Shared.DTOs.Common;
using Shared.DTOs.Servicos.Get;

namespace Application.Services.Servico
{
    public class GetServicesById
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServicesById(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public ServiceDtoOutput Execute(int id)
        {
            var service = _serviceRepository.GetServiceById(id);
            return new ServiceDtoOutput(
                service.Id,
                service.Name,
                service.Description,
                service.MaximumConcurrentAppointments,
                service.ResponsibleUser
                );
        }
    }
}