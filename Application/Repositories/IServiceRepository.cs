using Domain.LavaCar.Entitites;
using Shared.DTOs.Common;

namespace Application.Repositories
{
    public interface IServiceRepository
    {
        Service GetServiceById(int id);
        void Save(Service service);
        PagedResult<Service> GetByUser(string responsibleUser, int page, int pageSize);
    }
}
