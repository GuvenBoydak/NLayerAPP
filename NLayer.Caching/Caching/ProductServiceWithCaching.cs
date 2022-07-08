using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core;
using System.Linq.Expressions;

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productrepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository productrepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productrepository = productrepository;
            _unitOfWork = unitOfWork;

            //!_memoryCache.TryGetValue(CacheProductKey) ile CacheProductKey deger varsa almaya çalişyoruz.
            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
                //Deger yok ise CacheProductKey e _productrepository.GetAll().ToList() u set ediyoruz.
                _memoryCache.Set(CacheProductKey, _productrepository.GetProductsWithCategory().Result);
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productrepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productrepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            return Task.FromResult(_memoryCache.Get<List<Product>>(CacheProductKey).Any(expression.Compile()));
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public  Task<Product> GetByIdAsync(int id)
        {
            Product product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);
            if (product == null)
                throw new NotFoundException($"{typeof(Product).Name}({id}) Not Found");

            return Task.FromResult(product);
        }

        public  Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            IEnumerable<Product> products =_memoryCache.Get<IEnumerable<Product>>(CacheProductKey);

            List<ProductWithCategoryDto> productsWithCategoy = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategoy));
        }

        public async Task RemoveAsync(Product entity)
        {
             _productrepository.Remove(entity);
            await  _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _productrepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _productrepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public  async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(CacheProductKey, await _productrepository.GetAll().ToListAsync());
        }
    }
}
