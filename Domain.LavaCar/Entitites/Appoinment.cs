using System;

namespace Domain.LavaCar.Entitites
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ClientName { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ResponsibleUser { get; set; }

        public Appointment() { }

        public Appointment(int id, int serviceId, string clientName, DateTime scheduledDateTime, bool isActive, DateTime createdAt, DateTime updatedAt, string responsibleUser)
        {
            Id = id;
            ServiceId = serviceId;
            ClientName = clientName;
            ScheduledDateTime = scheduledDateTime;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ResponsibleUser = responsibleUser;
        }

        public static Appointment Create(int serviceId, string clientName, DateTime scheduledDateTime, string responsibleUser)
        {
            return new Appointment
            {
                ServiceId = serviceId,
                ClientName = clientName,
                ScheduledDateTime = scheduledDateTime,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ResponsibleUser = responsibleUser
            };
        }

        public void Disable()
        {
            IsActive = false;
            UpdatedAt = DateTime.Now;
        }
    }

}
