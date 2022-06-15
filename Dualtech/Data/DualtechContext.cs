using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dualtech.Modelos;

namespace Dualtech.Data
{
    public class DualtechContext : DbContext
    {
        public DualtechContext (DbContextOptions<DualtechContext> options)
            : base(options)
        {
        }

        public DbSet<Dualtech.Modelos.Cliente> Cliente { get; set; }

        public DbSet<Dualtech.Modelos.Producto> Producto { get; set; }

        public DbSet<Dualtech.Modelos.Orden> Orden { get; set; }

        public DbSet<Dualtech.Modelos.DetalleOrden> DetalleOrden { get; set; }
    }
}
