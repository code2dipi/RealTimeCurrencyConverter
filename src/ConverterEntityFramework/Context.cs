using ConverterEntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConverterEntityFramework
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options) 
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=Dipi;Database=CurrencyConverter;Trusted_Connection=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ConverterEntity>(builder =>
            {
                builder.ToTable(nameof(ConverterEntity))
                    .HasKey(ent => ent.Id);
                builder.Property(p => p.Id).ValueGeneratedOnAdd();

                builder.Property(p => p.DateTime).IsRequired();
                builder.Property(p => p.Value)
                    .HasColumnType("nvarchar(MAX)").IsRequired();
            });
        }
    }
}
