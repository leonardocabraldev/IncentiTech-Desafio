using Application.Repositories;
using Domain.LavaCar.Entitites;
using Shared.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Infrastructure.LavaCar.Data.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Appointment GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT Id, ServiceId, ClientName, AppointmentDateTime, IsActive, CreatedAt, UpdatedAt, ResponsibleUser 
                      FROM Appointments WHERE Id = @Id AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Appointment
                        {
                            Id = reader.GetInt32(0),
                            ServiceId = reader.GetInt32(1),
                            ClientName = reader.GetString(2),
                            ScheduledDateTime = reader.GetDateTime(3),
                            IsActive = reader.GetBoolean(4),
                            CreatedAt = reader.GetDateTime(5),
                            UpdatedAt = reader.GetDateTime(6),
                            ResponsibleUser = reader.GetString(7)
                        };
                    }
                }
            }
            return null;
        }

        public IEnumerable<Appointment> GetAllActive()
        {
            var items = new List<Appointment>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT Id, ServiceId, ClientName, AppointmentDateTime, IsActive, CreatedAt, UpdatedAt, ResponsibleUser 
                      FROM Appointments WHERE IsActive = 1", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Appointment
                        {
                            Id = reader.GetInt32(0),
                            ServiceId = reader.GetInt32(1),
                            ClientName = reader.GetString(2),
                            ScheduledDateTime = reader.GetDateTime(3),
                            IsActive = reader.GetBoolean(4),
                            CreatedAt = reader.GetDateTime(5),
                            UpdatedAt = reader.GetDateTime(6),
                            ResponsibleUser = reader.GetString(7)
                        });
                    }
                }
            }
            return items;
        }

        public IEnumerable<Appointment> GetByServiceId(int serviceId)
        {
            var items = new List<Appointment>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT Id, ServiceId, ClientName, AppointmentDateTime, IsActive, CreatedAt, UpdatedAt, ResponsibleUser 
                      FROM Appointments WHERE ServiceId = @ServiceId AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@ServiceId", serviceId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Appointment
                        {
                            Id = reader.GetInt32(0),
                            ServiceId = reader.GetInt32(1),
                            ClientName = reader.GetString(2),
                            ScheduledDateTime = reader.GetDateTime(3),
                            IsActive = reader.GetBoolean(4),
                            CreatedAt = reader.GetDateTime(5),
                            UpdatedAt = reader.GetDateTime(6),
                            ResponsibleUser = reader.GetString(7)
                        });
                    }
                }
            }
            return items;
        }

        public void Save(Appointment appointment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command;
                if (appointment.Id == 0)
                {
                    command = new SqlCommand(
                        @"INSERT INTO Appointments (ServiceId, ClientName, AppointmentDateTime, IsActive, CreatedAt, UpdatedAt, ResponsibleUser)
                          VALUES (@ServiceId, @ClientName, @AppointmentDateTime, @IsActive, @CreatedAt, @UpdatedAt, @ResponsibleUser)", connection);
                    command.Parameters.AddWithValue("@ServiceId", appointment.ServiceId);
                    command.Parameters.AddWithValue("@ClientName", appointment.ClientName);
                    command.Parameters.AddWithValue("@AppointmentDateTime", appointment.ScheduledDateTime);
                    command.Parameters.AddWithValue("@IsActive", appointment.IsActive);
                    command.Parameters.AddWithValue("@CreatedAt", appointment.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", appointment.UpdatedAt);
                    command.Parameters.AddWithValue("@ResponsibleUser", appointment.ResponsibleUser);
                    command.ExecuteNonQuery();
                }
                else
                {
                    command = new SqlCommand(
                        @"UPDATE Appointments SET
                            ServiceId = @ServiceId,
                            ClientName = @ClientName,
                            AppointmentDateTime = @AppointmentDateTime,
                            IsActive = @IsActive,
                            UpdatedAt = @UpdatedAt,
                            ResponsibleUser = @ResponsibleUser
                          WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", appointment.Id);
                    command.Parameters.AddWithValue("@ServiceId", appointment.ServiceId);
                    command.Parameters.AddWithValue("@ClientName", appointment.ClientName);
                    command.Parameters.AddWithValue("@AppointmentDateTime", appointment.ScheduledDateTime);
                    command.Parameters.AddWithValue("@IsActive", appointment.IsActive);
                    command.Parameters.AddWithValue("@UpdatedAt", appointment.UpdatedAt);
                    command.Parameters.AddWithValue("@ResponsibleUser", appointment.ResponsibleUser);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int CountSimultaneousAppointments(int serviceId, DateTime scheduledDateTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT COUNT(*) FROM Appointments 
                      WHERE ServiceId = @ServiceId AND AppointmentDateTime = @AppointmentDateTime AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@ServiceId", serviceId);
                command.Parameters.AddWithValue("@AppointmentDateTime", scheduledDateTime);
                return (int)command.ExecuteScalar();
            }
        }

        public bool ExistsAtSameTime(int serviceId, DateTime scheduledDateTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    @"SELECT COUNT(*) FROM Appointments 
                      WHERE ServiceId = @ServiceId AND AppointmentDateTime = @AppointmentDateTime AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@ServiceId", serviceId);
                command.Parameters.AddWithValue("@AppointmentDateTime", scheduledDateTime);
                return ((int)command.ExecuteScalar()) > 0;
            }
        }

        public PagedResult<Appointment> GetByUser(string responsibleUser, int page, int pageSize)
        {
            var items = new List<Appointment>();
            int totalCount = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var countCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM Appointments WHERE ResponsibleUser = @ResponsibleUser AND IsActive = 1", connection);
                countCommand.Parameters.AddWithValue("@ResponsibleUser", responsibleUser);
                totalCount = (int)countCommand.ExecuteScalar();

                var command = new SqlCommand(
                    @"SELECT Id, ServiceId, ClientName, AppointmentDateTime, IsActive, CreatedAt, UpdatedAt, ResponsibleUser
              FROM Appointments
              WHERE ResponsibleUser = @ResponsibleUser AND IsActive = 1
              ORDER BY AppointmentDateTime DESC, Id DESC
              OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
                command.Parameters.AddWithValue("@ResponsibleUser", responsibleUser);
                command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Appointment
                        {
                            Id = reader.GetInt32(0),
                            ServiceId = reader.GetInt32(1),
                            ClientName = reader.GetString(2),
                            ScheduledDateTime = reader.GetDateTime(3),
                            IsActive = reader.GetBoolean(4),
                            CreatedAt = reader.GetDateTime(5),
                            UpdatedAt = reader.GetDateTime(6),
                            ResponsibleUser = reader.GetString(7)
                        });
                    }
                }
            }

            return new PagedResult<Appointment>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public int CountTotalAppointments()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT COUNT(*) FROM Appointments WHERE IsActive = 1", connection);
                return (int)command.ExecuteScalar();
            }
        }
    }
}
