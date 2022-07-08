using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core;

namespace NLayer.API.Controllers
{

    public class CategoriesController : CustomeBaseController
    {

        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCategoryWithProducts([FromRoute] int id)
        {
            return CreateActionResult(await _service.GetSingleCategoryByIdWithProductsAsync(id));
        }
    }
}
