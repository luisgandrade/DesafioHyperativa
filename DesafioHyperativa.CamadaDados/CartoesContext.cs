using DesafioHyperativa.CamadaDados.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesafioHyperativa.CamadaDados
{
    public class CartoesContext : DbContext
    {
        public DbSet<Cartao> Cartoes { get; set; }

        public CartoesContext(DbContextOptions<CartoesContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cartao>();
        }
    }
}
