using MediportaKMZadanieRekrutacyjne.Models;
using Microsoft.EntityFrameworkCore;

namespace MediportaKMZadanieRekrutacyjne.Database
{
    public class SoApiDbContext : DbContext
    {
        public SoApiDbContext()
        {

        }

        public SoApiDbContext(DbContextOptions<SoApiDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SODB;Trusted_Connection=True;");

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
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
