using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatoriosController : ControllerBase
    {
        private readonly IRelatoriosRepository _repository;
        private readonly IRelatorioServices _services;

        public RelatoriosController(IRelatoriosRepository repository, IRelatorioServices services)
        {
            _repository = repository;
            _services = services;
        }

        [HttpPost("get-lista-voluntarios")]
        public async Task<Result<DadosRelatorio<List<CheckInReports>>>> GetListHomens(ParametrosEvento dto)
        {
            return await _repository.GetByIdHomens(dto);
        }

        [HttpPost("get-lista-conectados")]
        public async Task<Result<FichasDto<List<CheckInReports>>>> GetListConectados(ParametrosConectados dto)
        {
            return await _repository.GetByConectados(dto);
        }
    }
}
