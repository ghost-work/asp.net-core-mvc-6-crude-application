using Microsoft.EntityFrameworkCore;
using mvcCrudeApplication.Models.Domain;

namespace mvcCrudeApplication.Data
{
    public class MvcDemoTestDbContext : DbContext
    {
        public MvcDemoTestDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
