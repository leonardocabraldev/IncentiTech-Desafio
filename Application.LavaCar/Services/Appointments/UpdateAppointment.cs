using System;
using Application.Repositories;
using Shared.DTOs.Appointments.Update;

namespace Application.Services.Appointments
{
    public class UpdateAppointment
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;

        public UpdateAppointment(IAppointmentRepository appointmentRepository, IServiceRepository serviceRepository)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
        }

        public bool Execute(UpdateAppointmentInput input)
        {
            var current = _appointmentRepository.GetById(input.Id);
            if (current == null || !current.IsActive)
                return false;

            bool changingSlot = current.ServiceId != input.ServiceId
                                || current.ScheduledDateTime != input.ScheduledDateTime;

            if (changingSlot && _appointmentRepository.ExistsAtSameTime(input.ServiceId, input.ScheduledDateTime))
                return false;

            var svc = _serviceRepository.GetServiceById(input.ServiceId);
            if (svc == null || !svc.IsActive)
                return false;

            int totalAtSlot = _appointmentRepository.CountSimultaneousAppointments(input.ServiceId, input.ScheduledDateTime);

            if (!changingSlot && totalAtSlot > 0)
                totalAtSlot--;

            if (totalAtSlot >= svc.MaximumConcurrentAppointments)
                return false;

            current.Update(input.ServiceId, input.ClientName, input.ScheduledDateTime);

            _appointmentRepository.Save(current);

            return true;
        }
    }
}