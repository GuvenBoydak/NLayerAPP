using Microsoft.EntityFrameworkCore;
using NLayer.Core;

namespace NLayer.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<Category> GetSingleCategoryByIdWithProductsAsync(int id)
        {
            return await _db.Categories.Include(x => x.Products).Where(x => x.Id == id).SingleOrDefaultAsync();
                
        }
    }
}
