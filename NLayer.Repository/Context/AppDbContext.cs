using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using System.Reflection;

namespace NLayer.Repository
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductFeature> ProductFeatures { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Assembly'deki Tüm configuration dosylarını okuyor. IEntityTypeConfiguration'den implemente eden classları reflection sayesinde buluyor.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Configurationları tek tek uygulamak için ise bu şekilde yazıyoruz.
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            //İstersek seed dataları bu şekilde ekleyebiliriz.
            //Fakat modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); yazdıgımız kod ile seeds Klasorundeki seed datalari  IEntityTypeConfiguration'den implemente etigimiz için reflection sayesinde okunup işlenicek.
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                Id = 1,
                Color = "mavi",
                Height = 100,
                Width = 20,
                ProductId = 1
            });
        }
    }
}
