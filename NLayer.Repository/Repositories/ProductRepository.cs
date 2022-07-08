using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<List<Product>> GetProductsWithCategory()
        {
            return await _db.Products.Include(x => x.Category).ToListAsync();
        }
    }
}
