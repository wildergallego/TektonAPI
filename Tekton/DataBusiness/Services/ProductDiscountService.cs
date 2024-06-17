using BusinessLayer.Common;
using BusinessLayer.Dto;
using BusinessLayer.Interfaces;
using System.Text.Json;

namespace BusinessLayer.Services
{
    public class ProductDiscountService : IProductDiscount
    {
        readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions caseInsentitive = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ProductDiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> GetDiscountById(int idProduct)
        {
            try
            {
                var respuesta = await this._httpClient.GetAsync(string.Concat(this._httpClient.BaseAddress, idProduct.ToString()));
                var contenidoRespuesta = await respuesta.Content.ReadAsStringAsync();

                var json = JsonSerializer.Deserialize<ProductDiscountDto>(contenidoRespuesta, caseInsentitive);

                if (json == null)
                {
                    return Constants.RETURNDEFAULT;
                }

                return json.discountPercent;
            }
            catch (Exception)
            {
                return Constants.RETURNDEFAULT;
            }
        }
    }
}
