using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Amigos.Domain;

namespace Api.Amigos.Data
{
    public class ApiAmigosContext : DbContext
    {
        public ApiAmigosContext (DbContextOptions<ApiAmigosContext> options)
            : base(options)
        {
        }

        public DbSet<Api.Amigos.Domain.Amigos> Amigos { get; set; }
    }
}
