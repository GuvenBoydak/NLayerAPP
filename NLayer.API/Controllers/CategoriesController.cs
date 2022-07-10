using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core;

namespace NLayer.API.Controllers
{

    public class CategoriesController : CustomeBaseController
    {

        private readonly ICategoryService _service;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategoryWithProducts([FromRoute] int id)
        {
            return CreateActionResult(await _service.GetSingleCategoryByIdWithProductsAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<Category> categories = await _service.GetAllAsync();
            List<CategoryDto> categoryDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            return CreateActionResult(CustomResponseDto<List<CategoryDto>>.Success(200, categoryDto));
        }
    }
}
