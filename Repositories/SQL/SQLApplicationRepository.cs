using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIMS.Data;
using AIMS.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIMS.Repositories.SQL
{

    public class SQLApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext context;
        public SQLApplicationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddAsync(Application application)
        {
            context.Applications.Add(application);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected;
        }

        public void Delete(int Id)
        {
            Application application = context.Applications.Find(Id);
            if (application != null)
            {
                //context.Applications.Remove(application);
                //context.SaveChanges();
                application.Status = "Disabled";
            }
            Update(application);
        }

        public bool Get(int id, out Application application)
        {
            application = context.Applications.Find(id);
            return (application != null);
        }

        public PagedList<Application> GetAll([FromQuery]PagingParameters parameters, HttpContext httpContext)
        {
            var source = (from solution in context.Applications.OrderBy(a => a.Id) select solution).AsQueryable().Where(x => x.Status == "Enabled");

            return PagedList<Application>.ToPagedList(source, parameters.PageNumber, parameters.PageSize, httpContext);
        }

        public Application Update(Application changes)
        {
            var Application = context.Applications.Attach(changes);
            Application.State = EntityState.Modified;
            context.SaveChanges();
            return changes;
        }
    }
}
