using Application.Repositories;
using Domain.LavaCar.Entitites;
using Shared.DTOs.Appointments.Create;

namespace Application.Services.Appointments
{
    public class CreateAppointments
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public CreateAppointments(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public void Execute(CreateAppointmentInput input)
        {
            var newAppointment = Appointment.Create(input.ServiceId, input.ClientName, input.ScheduledDateTime, input.ResponsibleUser);
            _appointmentRepository.Save(newAppointment);
        }
    }
}
