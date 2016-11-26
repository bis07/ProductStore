using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityLibrary1.Entities;

namespace ActivityLibrary1.Concrete
{
    class EFDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; } 
    }
}
