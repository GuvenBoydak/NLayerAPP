using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IProductService productService, IUnitOfWork unitOfWork, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> Save()
        {
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name","Seciniz");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Product product = await _productService.GetByIdAsync(id);

            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name",product.CategoryId);

            return View(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if(ModelState.IsValid)
            {
                await _productService.UpdateAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            List<CategoryDto> categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Product prodcut = await _productService.GetByIdAsync(id);
           await _productService.RemoveAsync(prodcut);
            return RedirectToAction(nameof(Index));
        }
 

    }
}
