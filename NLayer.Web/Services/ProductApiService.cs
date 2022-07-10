using NLayer.Core;

namespace NLayer.Web
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<ProductWithCategoryDto>> GetProductWithCategoriesAsync()
        {
            CustomResponseDto<List<ProductWithCategoryDto>> response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("products/GetProductsWithCategory");
            return response.Data;
        }

        public async Task<ProductDto> SaveAsync(ProductDto productDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("products", productDto);

            if (!response.IsSuccessStatusCode)
                return null;
            CustomResponseDto<ProductDto> responceBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();

            return responceBody.Data;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            CustomResponseDto<ProductDto> response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");
            return response.Data;
        }

        public async Task<bool> UpdateAsync(ProductDto productDto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync("products", productDto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"products/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
