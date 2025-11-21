namespace Shared.DTOs.Servicos.Create
{
    public class CreateServiceOutput
    {
        public CreateServiceOutput(string name, string description, int maximumConcurrentAppointments, string responsibleUser)
        {
            Name = name;
            Description = description;
            MaximumConcurrentAppointments = maximumConcurrentAppointments;
            ResponsibleUser = responsibleUser;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int MaximumConcurrentAppointments { get; set; }
        public string ResponsibleUser { get; set; }
    }
}
