using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaPasantia.Models
{
    public class DbContexto : DbContext 
    {
        public DbContexto(DbContextOptions<DbContexto> options) : base(options)
        {

        }
        public DbSet<Auto> Autos { get; set; }

    }
}
