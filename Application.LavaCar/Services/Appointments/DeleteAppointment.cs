using Application.Repositories;

namespace Application.Services.Appointments
{
    public class DeleteAppointment
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public DeleteAppointment(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public bool Execute(int appointmentId)
        {
            var appointment = _appointmentRepository.GetById(appointmentId);
            if (appointment == null)
                return false;

            appointment.Disable();
            _appointmentRepository.Save(appointment);
            return true;
        }
    }
}