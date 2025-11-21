namespace Shared.DTOs.Appointments.Update
{
    public class UpdateAppointmentInput
    {
        public UpdateAppointmentInput(int id, int serviceId, string clientName, System.DateTime scheduledDateTime, string responsibleUser)
        {
            Id = id;
            ServiceId = serviceId;
            ClientName = clientName;
            ScheduledDateTime = scheduledDateTime;
            ResponsibleUser = responsibleUser;
        }

        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ClientName { get; set; }
        public System.DateTime ScheduledDateTime { get; set; }
        public string ResponsibleUser { get; set; }
    }
}