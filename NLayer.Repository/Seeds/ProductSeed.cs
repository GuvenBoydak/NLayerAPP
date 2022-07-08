using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;

namespace NLayer.Repository
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product() { Id = 1, CategoryId = 1, Price = 100, Stock = 20, Name = "Tükenmez kalem" },
                new Product() { Id = 2, CategoryId = 1, Price = 100, Stock = 20, Name = "Kurşun kalem" },
                new Product() { Id = 3, CategoryId = 1, Price = 100, Stock = 20, Name = "Uçlu kalem" },
                new Product() { Id = 4, CategoryId = 2, Price = 120, Stock = 10, Name = "Polisiye Kitap" },
                new Product() { Id = 5, CategoryId = 2, Price = 125, Stock = 15, Name = "Macera Kitap" },
                new Product() { Id = 6, CategoryId = 3, Price = 150, Stock = 25, Name = "Kareli defter" },
                new Product() { Id = 7, CategoryId = 3, Price = 120, Stock = 20, Name = "Çizgili defter" }
                );
        }
    }
}
