using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using System.Linq.Expressions;

namespace NLayer.Caching
{
    public class CategoryServiceWithCaching:ICategoryService
    {
        private const string CacheCategoryKey = "categoryCache";
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;

        public CategoryServiceWithCaching(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            if (!_memoryCache.TryGetValue(CacheCategoryKey, out _))
                _memoryCache.Set(CacheCategoryKey, _categoryRepository.GetAll().ToListAsync());
        }

        public async Task<Category> AddAsync(Category entity)
        {
            await _categoryRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllCategories();
            return entity;
        }

        public async Task<IEnumerable<Category>> AddRangeAsync(IEnumerable<Category> entities)
        {
            await _categoryRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllCategories();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Category, bool>> expression)
        {
            return Task.FromResult(_memoryCache.Get<List<Category>>(CacheCategoryKey).Any(expression.Compile()));
        }

        public  Task<IEnumerable<Category>> GetAllAsync()
        {
            return  Task.FromResult(_memoryCache.Get<IEnumerable<Category>>(CacheCategoryKey));
        }

        public Task<Category> GetByIdAsync(int id)
        {
            Category category = _memoryCache.Get<List<Category>>(CacheCategoryKey).FirstOrDefault(x => x.Id == id);
            if (category == null)
                throw new NotFoundException($"{typeof(Category).Name}({id}) Not Found");

            //Task dönen metodlarda eger asycn bir yapı kullanımıyorsa Task.FromResult() kullanmalıyız.
            return Task.FromResult(category);
        }

        public async Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductsAsync(int id)
        {
            Category category = await _categoryRepository.GetSingleCategoryByIdWithProductsAsync(id);

            CategoryWithProductsDto categoryDto = _mapper.Map<CategoryWithProductsDto>(category);

            return CustomResponseDto<CategoryWithProductsDto>.Success(200, categoryDto);
        }

        public async Task RemoveAsync(Category entity)
        {
            _categoryRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllCategories();
        }

        public async Task RemoveRangeAsync(IEnumerable<Category> entities)
        {
            _categoryRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllCategories();
        }

        public async Task UpdateAsync(Category entity)
        {
            _categoryRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllCategories();
        }

        public IQueryable<Category> Where(Expression<Func<Category, bool>> expression)
        {
            return _memoryCache.Get<List<Category>>(CacheCategoryKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllCategories()
        {
            _memoryCache.Set(CacheCategoryKey, await _categoryRepository.GetAll().ToListAsync());
        }
    }
}
