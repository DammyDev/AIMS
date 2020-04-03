using Microsoft.AspNetCore.Http;
using ProjectAPI.Data;
using ProjectAPI.Pagination;
using System.Threading.Tasks;

namespace ProjectAPI.Repositories
{
    public interface IApplicationRepository
    {
        bool Get(int Id, out Application application);
        PagedList<Application> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Application application);
        Application Update(Application changes);
        void Delete(int Id);
    }
}
