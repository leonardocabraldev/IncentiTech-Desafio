namespace Infrastructure.LavaCar.Data.Repository
{
    using Application.Repositories;
    using Domain.LavaCar.Entitites;
    using Shared.DTOs.Common;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    namespace Infrastructure.Repositories
    {
        public class ServiceRepository : IServiceRepository
        {
            private readonly string _connectionString;

            public ServiceRepository(string connectionString)
            {
                _connectionString = connectionString;
            }

            public Service GetServiceById(int id)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "SELECT Id, Name, Description, MaximumConcurrentAppointments, IsActive, CreatedAt, UpdatedAt, ResponsibleUser FROM Services WHERE Id = @Id AND IsActive = 1", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Service(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(3),
                                reader.GetBoolean(4),
                                reader.GetDateTime(5),
                                reader.GetDateTime(6),
                                reader.GetString(7)
                            );
                        }
                    }
                }
                return null;
            }

            public void Save(Service service)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command;
                    if (service.Id == 0)
                    {
                        command = new SqlCommand(
                            @"INSERT INTO Services (Name, Description, MaximumConcurrentAppointments, IsActive, CreatedAt, UpdatedAt, ResponsibleUser)
                          VALUES (@Name, @Description, @MaximumConcurrentAppointments, @IsActive, @CreatedAt, @UpdatedAt, @ResponsibleUser)", connection);
                        command.Parameters.AddWithValue("@Name", service.Name);
                        command.Parameters.AddWithValue("@Description", service.Description);
                        command.Parameters.AddWithValue("@MaximumConcurrentAppointments", service.MaximumConcurrentAppointments);
                        command.Parameters.AddWithValue("@IsActive", service.IsActive);
                        command.Parameters.AddWithValue("@CreatedAt", service.CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", service.UpdatedAt);
                        command.Parameters.AddWithValue("@ResponsibleUser", service.ResponsibleUser);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command = new SqlCommand(
                            @"UPDATE Services SET
                            Name = @Name,
                            Description = @Description,
                            MaximumConcurrentAppointments = @MaximumConcurrentAppointments,
                            IsActive = @IsActive,
                            UpdatedAt = @UpdatedAt,
                            ResponsibleUser = @ResponsibleUser
                          WHERE Id = @Id", connection);
                        command.Parameters.AddWithValue("@Id", service.Id);
                        command.Parameters.AddWithValue("@Name", service.Name);
                        command.Parameters.AddWithValue("@Description", service.Description);
                        command.Parameters.AddWithValue("@MaximumConcurrentAppointments", service.MaximumConcurrentAppointments);
                        command.Parameters.AddWithValue("@IsActive", service.IsActive);
                        command.Parameters.AddWithValue("@UpdatedAt", service.UpdatedAt);
                        command.Parameters.AddWithValue("@ResponsibleUser", service.ResponsibleUser);
                        command.ExecuteNonQuery();
                    }
                }
            }

            public PagedResult<Service> GetByUser(string responsibleUser, int page, int pageSize)
            {
                var items = new List<Service>();
                int totalCount = 0;

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var countCommand = new SqlCommand(
                        "SELECT COUNT(*) FROM Services WHERE ResponsibleUser = @ResponsibleUser", connection);
                    countCommand.Parameters.AddWithValue("@ResponsibleUser", responsibleUser);
                    totalCount = (int)countCommand.ExecuteScalar();

                    var command = new SqlCommand(
                        @"SELECT Id, Name, Description, MaximumConcurrentAppointments, IsActive, CreatedAt, UpdatedAt, ResponsibleUser
                      FROM Services
                      WHERE ResponsibleUser = @ResponsibleUser AND IsActive = 1
                      ORDER BY Id
                      OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
                    command.Parameters.AddWithValue("@ResponsibleUser", responsibleUser);
                    command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Service(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(3),
                                reader.GetBoolean(4),
                                reader.GetDateTime(5),
                                reader.GetDateTime(6),
                                reader.GetString(7)
                            ));
                        }
                    }
                }

                return new PagedResult<Service>
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize
                };
            }
        }
    }

}
