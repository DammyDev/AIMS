using Microsoft.AspNetCore.Http;
using AIMS.Data;
using AIMS.Pagination;
using System.Threading.Tasks;

namespace AIMS.Repositories
{
    public interface IDatabaseRepository
    {
        bool Get(int Id, out Database database);
        PagedList<Database> GetAll(PagingParameters parameters, HttpContext httpContext);
        Task<int> AddAsync(Database database);
        Database Update(Database changes);
        void Delete(int Id);
    }
}
