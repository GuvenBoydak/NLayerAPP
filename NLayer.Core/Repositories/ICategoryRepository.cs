namespace NLayer.Core
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<Category> GetSingleCategoryByIdWithProductsAsync(int id);
    }
}
