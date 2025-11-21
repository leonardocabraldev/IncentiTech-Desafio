using System;

namespace Domain.LavaCar.Entitites
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaximumConcurrentAppointments { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string ResponsibleUser { get; set; }
        public Service(int id, string name, string description, int maximumConcurrentAppointments, bool isActive, DateTime createdAt, DateTime updatedAt, string responsibleUser)
        {
            Id = id;
            Name = name;
            Description = description;
            MaximumConcurrentAppointments = maximumConcurrentAppointments;
            IsActive = isActive;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            ResponsibleUser = responsibleUser;
        }

        public static Service Create(string name, string description, int maximumConcurrentAppointments, string responsibleUser)
        {
            return new Service(
                0,
                name,
                description, 
                maximumConcurrentAppointments,
                true,
                DateTime.UtcNow,
                DateTime.UtcNow,
                responsibleUser
                );
        }

        public void Disable()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
