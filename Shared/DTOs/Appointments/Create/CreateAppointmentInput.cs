using System;

namespace Shared.DTOs.Appointments.Create
{
    public class CreateAppointmentInput
    {
        public CreateAppointmentInput(int serviceId, string clientName, DateTime scheduledDateTime, string responsibleUser)
        {
            ServiceId = serviceId;
            ClientName = clientName;
            ScheduledDateTime = scheduledDateTime;
            ResponsibleUser = responsibleUser;
        }

        public int ServiceId { get; set; }
        public string ClientName { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string ResponsibleUser { get; set; }
    }
}
