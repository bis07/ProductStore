using System.Data.Entity;
using ActivityLibrary1.Entities;

namespace ActivityLibrary1.Concrete
{
    class EFDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; } 
    }
}
