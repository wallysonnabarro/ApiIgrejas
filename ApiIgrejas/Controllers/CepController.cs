using Domain.Dominio;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CepController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CepController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("cep")]
        public async Task<IActionResult> Detalhar(CepDto cep)
        {
            var url = $"https://viacep.com.br/ws/{cep.Cep}/json/";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cepData = JsonConvert.DeserializeObject<Cep>(content);
                return Ok(cepData);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Erro ao obter informações do CEP.");
            }
        }
    }
}
