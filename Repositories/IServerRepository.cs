using Microsoft.AspNetCore.Http;
using AIMS.Data;
using AIMS.Pagination;
using System.Threading.Tasks;

namespace AIMS.Repositories
{
    public interface IServerRepository
    {
        bool Get(int Id, out Server server);
        PagedList<Server> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Server server);
        Server Update(Server server);
        void Delete(int Id);
    }
}
