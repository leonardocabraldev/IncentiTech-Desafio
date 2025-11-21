using Application.Repositories;

namespace Application.Services.Servico
{
    public class DeleteService
    {
        private readonly IServiceRepository _serviceRepository;

        public DeleteService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public bool Execute(int serviceId)
        {
            var service = _serviceRepository.GetServiceById(serviceId);
            if (service == null)
                return false;

            service.Disable();

            _serviceRepository.Save(service);
            return true;
        }
    }
}
