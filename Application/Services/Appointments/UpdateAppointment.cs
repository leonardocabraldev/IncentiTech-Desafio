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
            // Recupera agendamento atual
            var current = _appointmentRepository.GetById(input.Id);
            if (current == null || !current.IsActive) return false;

            // Regras: data futura e antecedência mínima de 2h
            var now = DateTime.Now;
            if (input.ScheduledDateTime <= now) return false;
            if (input.ScheduledDateTime < now.AddHours(2)) return false;

            // Regra: não permitir mesmo serviço no mesmo horário (exceto se não mudou)
            var changingSlot = current.ServiceId != input.ServiceId || current.ScheduledDateTime != input.ScheduledDateTime;
            if (changingSlot && _appointmentRepository.ExistsAtSameTime(input.ServiceId, input.ScheduledDateTime))
                return false;

            // Regra: respeitar limite de simultâneos
            var service = _serviceRepository.GetByUser(current.ResponsibleUser, 1, 1); // não usado
            var svc = _serviceRepository.GetServiceById(input.ServiceId);
            if (svc == null || !svc.IsActive) return false;

            var totalAtSlot = _appointmentRepository.CountSimultaneousAppointments(input.ServiceId, input.ScheduledDateTime);
            // Se o slot não mudou, o COUNT inclui o próprio registro: ajusta
            if (!changingSlot && totalAtSlot > 0) totalAtSlot -= 1;
            if (totalAtSlot >= svc.MaximumConcurrentAppointments) return false;

            // Atualiza campos
            current.ServiceId = input.ServiceId;
            current.ClientName = input.ClientName?.Trim();
            current.ScheduledDateTime = input.ScheduledDateTime;
            current.ResponsibleUser = input.ResponsibleUser;
            current.UpdatedAt = DateTime.Now;
            current.IsActive = true;

            _appointmentRepository.Save(current);
            return true;
        }
    }
}