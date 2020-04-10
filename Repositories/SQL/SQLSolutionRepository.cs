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
    public class SQLSolutionRepository : ISolutionRepository
    {
        private readonly AppDbContext context;
        public SQLSolutionRepository(AppDbContext context)
        {
            this.context = context;
        }


        public async Task<int> AddAsync(Solution solution)
        {
            context.Solution.Add(solution);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected;
        }

        public void Delete(int Id)
        {
            Solution solution = context.Solution.Find(Id);
            if (solution != null)
            {
                //context.Solution.Remove(solution);
                //context.SaveChanges();
                solution.Status = "Disabled";
            }
            Update(solution);
        }

        public bool Get(int id, out Solution solution)
        {
            solution = context.Solution.Find(id);
            return (solution != null);
        }

        public PagedList<Solution_> GetAll([FromQuery]PagingParameters parameters, HttpContext httpContext)
        {

            // Return List of Solutions  
            var source = (from solution in context.Solution.OrderBy(a => a.Id) select solution).AsQueryable().Where(x => x.Status == "Enabled");

            return PagedList<Solution_>.ToPagedList(source, parameters.PageNumber, parameters.PageSize, httpContext);
        }

        public Solution Update(Solution changes)
        {
            var solution = context.Solution.Attach(changes);
            solution.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
