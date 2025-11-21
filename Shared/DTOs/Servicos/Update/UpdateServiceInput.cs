namespace Shared.DTOs.Servicos.Update
{
    public class UpdateServiceInput
    {
        public UpdateServiceInput(int id, string name, string description, int maximumConcurrentAppointments, string responsibleUser)
        {
            Id = id;
            Name = name;
            Description = description;
            MaximumConcurrentAppointments = maximumConcurrentAppointments;
            ResponsibleUser = responsibleUser;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaximumConcurrentAppointments { get; set; }
        public string ResponsibleUser { get; set; }
    }
}
