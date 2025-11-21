using Application.Repositories;
using Domain.LavaCar.Entitites;
using Shared.DTOs.Appointments.Create;

namespace Application.Services.Appointments
{
    public class CreateAppointments
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;
        public CreateAppointments(IAppointmentRepository appointmentRepository, IServiceRepository serviceRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
        }

        public bool Execute(CreateAppointmentInput input)
        {
            var svc = _serviceRepository.GetServiceById(input.ServiceId);
            if (svc == null || !svc.IsActive)
                return false;

            int totalAtSlot = _appointmentRepository.CountSimultaneousAppointments(input.ServiceId, input.ScheduledDateTime);

            if (totalAtSlot >= svc.MaximumConcurrentAppointments)
                return false;

            var newAppointment = Appointment.Create(
                input.ServiceId,
                input.ClientName,
                input.ScheduledDateTime,
                input.ResponsibleUser
            );

            _appointmentRepository.Save(newAppointment);

            return true;
        }
    }
}
