using MediportaKMZadanieRekrutacyjne.Interfaces;
using MediportaKMZadanieRekrutacyjne.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediportaKMZadanieRekrutacyjne.Test
{
    public class InMemoryDbContext : DbContext, IDbCtx
    {
        public InMemoryDbContext() { }
        public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options) { }

        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseInMemoryDatabase("sodbTest");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.TagID);
                entity.ToTable("Tags");
                entity.Property(t => t.TagID).IsRequired();
                entity.Property(t => t.HasSynonyms).HasColumnType("bit");
                entity.Property(t => t.IsModeratorOnly).HasColumnType("bit");
                entity.Property(t => t.IsRequired).HasColumnType("bit");
                entity.Property(t => t.Count).HasColumnType("int");
                entity.Property(t => t.Name).HasColumnType("nvarchar(max)");
                entity.Property(t => t.PopulationPercentage).HasColumnType("decimal(7, 5)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
