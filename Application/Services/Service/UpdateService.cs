using Application.Repositories;
using Shared.DTOs.Servicos.Update;

namespace Application.Services.Servico
{
    public class UpdateService
    {
        private readonly IServiceRepository _serviceRepository;

        public UpdateService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public bool Execute(UpdateServiceInput input)
        {
            var service = _serviceRepository.GetServiceById(input.Id);
            if (service == null)
                return false;

            service.Name = input.Name;
            service.Description = input.Description;
            service.MaximumConcurrentAppointments = input.MaximumConcurrentAppointments;

            _serviceRepository.Save(service);
            return true;
        }
    }
}
