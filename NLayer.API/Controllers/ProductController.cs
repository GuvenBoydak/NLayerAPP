using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core;

namespace NLayer.API.Controllers
{

    public class ProductController : CustomeBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _service;

        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _service = productService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Product> product = await _service.GetAllAsync();

            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(product.ToList());

            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productDtos));
        }

        //Bir Filter constractorda bir parametre alıyorsa ServiceFilter üzerindne kullanılmalı ve program.cs e service e eklememiz gerekiyor.
        //ServiceFilter a Yazdıgımız NotFoundFilter i tip olarak veriyoruz.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            Product product = await _service.GetByIdAsync(id);

            ProductDto productDto = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            Product product = await _service.AddAsync(_mapper.Map<Product>(productDto));

            ProductDto response = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, response));
        }

        //Bir Filter constractorda bir parametre alıyorsa ServiceFilter üzerindne kullanılmalı ve program.cs e service e eklememiz gerekiyor.
        //ServiceFilter a Yazdıgımız NotFoundFilter i tip olarak veriyoruz.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productUpdateDto));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        //Bir Filter constractorda bir parametre alıyorsa ServiceFilter üzerindne kullanılmalı ve program.cs e service e eklememiz gerekiyor.
        //ServiceFilter a Yazdıgımız NotFoundFilter i tip olarak veriyoruz.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _service.GetByIdAsync(id);

            await _service.RemoveAsync(product);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
