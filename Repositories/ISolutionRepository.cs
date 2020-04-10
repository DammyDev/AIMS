using Microsoft.AspNetCore.Http;
using AIMS.Data;
using AIMS.Pagination;
using System.Threading.Tasks;

namespace AIMS.Repositories
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
