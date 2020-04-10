using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIMS.Data;
using AIMS.Pagination;
using System.Linq;
using System.Threading.Tasks;

namespace AIMS.Repositories.SQL
{
    public class SQLServerRepository : IServerRepository
    {
        private readonly AppDbContext context;
        public SQLServerRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Server server)
        {
            context.ServerInfo.Add(server);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected;
        }

        public void Delete(int Id)
        {
            Server server = context.ServerInfo.Find(Id);
            if (server != null)
            {
                //context.ServerInfo.Remove(server);
                //context.SaveChanges();
                server.Status = "Disabled";
            }
            Update(server);
        }

        public bool Get(int id, out Server server)
        {
            server = context.ServerInfo.Find(id);
            return (server != null);
        }

        public PagedList<Server> GetAll([FromQuery]PagingParameters parameters, HttpContext httpContext)
        {
            var source = (from solution in context.ServerInfo.OrderBy(a => a.Id) select solution).AsQueryable().Where(x => x.Status == "Enabled");

            return PagedList<Server>.ToPagedList(source, parameters.PageNumber, parameters.PageSize, httpContext);
        }

        public Server Update(Server changes)
        {
            var Database = context.ServerInfo.Attach(changes);
            Database.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return changes;
        }
    }
}
