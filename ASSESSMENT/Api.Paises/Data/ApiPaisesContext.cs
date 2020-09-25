using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Paises.Domain;

namespace Api.Paises.Data
{
    public class ApiPaisesContext : DbContext
    {
        public ApiPaisesContext (DbContextOptions<ApiPaisesContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pais>().HasMany(x => x.Estados);
        }

        public DbSet<Pais> Pais { get; set; }
        public DbSet<Estado> Estados { get; set; }
    }
}
