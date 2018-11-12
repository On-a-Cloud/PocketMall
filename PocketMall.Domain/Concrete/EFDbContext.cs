using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocketMall.Domain.Entities;
using System.Data.Entity;

namespace PocketMall.Domain.Concrete
{
    public class EFDbContext : System.Data.Entity.DbContext
    {
        public EFDbContext()
            : base("Name=EFDbContext")
        { }

        public DbSet<Product> Products { get; set; }
    }
}
