using Application.Repositories;
using Domain.LavaCar.Entitites;
using Shared.DTOs.Servicos.Create;

namespace Application.Services.Servico
{
    public class CreateService
    {
        private IServiceRepository _serviceRepository;
        public CreateService(IServiceRepository serviceRepository) 
        { 
            _serviceRepository = serviceRepository;
        }
        public CreateServiceOutput Execute(CreateServiceInput input)
        {
            var newService = Service.Create(input.Name,input.Description, input.MaximumConcurrentAppointments, input.ResponsibleUser);

            _serviceRepository.Save(newService);

            return new CreateServiceOutput(
                newService.Name,
                newService.Description,
                newService.MaximumConcurrentAppointments,
                newService.ResponsibleUser
                );
        }
    }
}
