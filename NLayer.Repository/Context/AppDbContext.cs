using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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


        public override int SaveChanges()
        {
            //Tracke edilen degişen entries leride dolaş.
            foreach (EntityEntry item in ChangeTracker.Entries())
            {
                //item.Entity BaseEntity ise 
                if(item.Entity is BaseEntity entityReferences)
                {
                    switch (item.State)
                    {  //item.State EntityState.Added ise  entityReferences in createdDate ine şuanki tarihi ata.
                        case EntityState.Added:
                            {
                                entityReferences.CreatedDate = DateTime.Now;
                                break;
                            }
                        //item.State EntityState.Modified ise  entityReferences in UpdatedDate ine şuanki tarihi ata.
                        case EntityState.Modified:
                            {
                                //entityReferences in createdDate ini güncelleme yaparken degiştirme diyoruz.
                                Entry(entityReferences).Property(x => x.CreatedDate).IsModified = false;

                                entityReferences.UpdatedDate = DateTime.Now;
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (EntityEntry item in ChangeTracker.Entries())
            {
                if(item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }              
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                        default:
                            break;
                    }
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }


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
