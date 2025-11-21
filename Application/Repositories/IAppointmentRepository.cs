using Domain.LavaCar.Entitites;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;

namespace Application.Repositories
{
    public interface IAppointmentRepository
    {
        Appointment GetById(int id);
        IEnumerable<Appointment> GetAllActive();
        IEnumerable<Appointment> GetByServiceId(int serviceId);
        void Save(Appointment appointment);
        int CountSimultaneousAppointments(int serviceId, DateTime scheduledDateTime);
        bool ExistsAtSameTime(int serviceId, DateTime scheduledDateTime);
        PagedResult<Appointment> GetByUser(string responsibleUser, int page, int pageSize);
    }
}
