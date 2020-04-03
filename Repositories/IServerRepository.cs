using Microsoft.AspNetCore.Http;
using ProjectAPI.Data;
using ProjectAPI.Pagination;
using System.Threading.Tasks;

namespace ProjectAPI.Repositories
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
