using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Overdrive.Api.Data;
using Overdrive.Api.Models;

namespace Overdrive.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiHistoricoAlteracaoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ApiHistoricoAlteracaoController(ApiDbContext context)
        {
            _context = context;
        }

        #region GETS

        //*BUSCA REGISTRO DE HISTORICO DE ALTERACAO DA PESSOA.
        [HttpGet("{id}")]
        public async Task<ActionResult<List<HistoricoAlteracao>>> BuscaHistoricoPorIdPessoa(int id)
        {
            List<HistoricoAlteracao> historicoAlteracao = await _context.HistoricoAlteracaos.Where(x => x.IdPessoa == id).ToListAsync();

            if (historicoAlteracao.Count() == 0)
            {
                return NotFound(new { message = "Nenhum registro encontrado para o Id infomrado." });
            }

            return historicoAlteracao;
        }

        #endregion
    }
}
