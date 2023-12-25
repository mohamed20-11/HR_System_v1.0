using HR_System.Models;
using Microsoft.EntityFrameworkCore;

namespace HR_System.Data
{
    public class ApplicationDbContxt:DbContext
    {
        public ApplicationDbContxt(DbContextOptions<ApplicationDbContxt> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Employee>().ToTable("Employees", "HR");
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
