using Application.Repositories;
using Domain.LavaCar.Entitites;
using Shared.DTOs.Appointments.Get;
using Shared.DTOs.Common;
using System.Linq;

namespace Application.Services.Appointments
{
    public class GetAppointmentsByUser
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;

        public GetAppointmentsByUser(IAppointmentRepository repository, IServiceRepository serviceRepository)
        {
            _appointmentRepository = repository;
            _serviceRepository = serviceRepository;
        }

        public PagedResult<GetAppointmentDto> Execute(string responsibleUser, int page, int pageSize)
        {
            var result = _appointmentRepository.GetByUser(responsibleUser, page, pageSize);

            var mapServicesName = _serviceRepository.GetByUser(responsibleUser, 1, int.MaxValue)
                .Items.ToDictionary(s => s.Id, s => s.Name);

            return new PagedResult<GetAppointmentDto>
            {
                Items = result.Items.Select(a => new GetAppointmentDto
                {
                    Id = a.Id,
                    ServiceId = a.ServiceId,
                    ServiceName = mapServicesName.ContainsKey(a.ServiceId) ? mapServicesName[a.ServiceId] : "Unknown Service",
                    ClientName = a.ClientName,
                    ScheduledDateTime = a.ScheduledDateTime,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    ResponsibleUser = a.ResponsibleUser
                }),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
    }
}
