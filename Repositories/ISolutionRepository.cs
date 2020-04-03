using Microsoft.AspNetCore.Http;
using ProjectAPI.Data;
using ProjectAPI.Pagination;
using System.Threading.Tasks;

namespace ProjectAPI.Repositories
{
    public interface ISolutionRepository
    {
        bool Get(int Id, out Solution solution);
        PagedList<Solution_> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Solution solution);
        Solution Update(Solution changes);
        void Delete(int Id);
    }
}
