using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DeMaria.Models;

namespace DeMaria.Data
{
    public class CartorioContext : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<RegistroNascimento> RegistrosNascimento { get; set; }
        public DbSet<RegistroCasamento> RegistrosCasamento { get; set; }
        public DbSet<RegistroObito> RegistrosObito { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DataNascimento).IsRequired();
                entity.Property(e => e.NomePai).HasMaxLength(100);
                entity.Property(e => e.NomeMae).HasMaxLength(100);
                entity.Property(e => e.CpfPai).HasMaxLength(11);
                entity.Property(e => e.CpfMae).HasMaxLength(11);
            });

            modelBuilder.Entity<RegistroNascimento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DataRegistro).IsRequired();
                entity.HasOne(e => e.Registrado)
                    .WithMany()
                    .HasForeignKey(e => e.RegistradoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RegistroCasamento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DataRegistro).IsRequired();
                entity.Property(e => e.DataCasamento).IsRequired();
                entity.HasOne(e => e.Conjuge1)
                    .WithMany()
                    .HasForeignKey(e => e.Conjuge1Id)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Conjuge2)
                    .WithMany()
                    .HasForeignKey(e => e.Conjuge2Id)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<RegistroObito>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DataRegistro).IsRequired();
                entity.Property(e => e.DataObito).IsRequired();
                entity.HasOne(e => e.Falecido)
                    .WithMany()
                    .HasForeignKey(e => e.FalecidoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 