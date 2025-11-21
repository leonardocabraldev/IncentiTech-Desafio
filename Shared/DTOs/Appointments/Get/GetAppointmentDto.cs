using System;

namespace Shared.DTOs.Appointments.Get
{
    public class GetAppointmentDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ClientName { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ResponsibleUser { get; set; }
    }
}
