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
    public class ApiPessoaController : ControllerBase
    {

        private readonly ApiDbContext _context;

        public ApiPessoaController(ApiDbContext context)
        {
            _context = context;
        }

        #region GETS

        //*BUSCA TODAS AS PESSOAS.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> BuscaTodasPessoas()
        {
            return await _context.Pessoa.ToListAsync();
        }

        //*BUSCA PESSOA PELO ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> BuscaPessoa(int id)
        {
            var pessoa = await _context.Pessoa.FindAsync(id);

            return pessoa == null ? NotFound() : pessoa;
        }

        //*BUSCA PESSOA PELO NOME
        [HttpGet("{nome}/nomePessoa")]
        public async Task<ActionResult<Pessoa>> BuscaPessoaPorNome(string nome)
        {
            var pessoa = await _context.Pessoa.Where(x => x.Nome == nome).FirstOrDefaultAsync();

            return pessoa == null ? NotFound() : pessoa;
        }

        #endregion

        #region POSTS

        //*ALTERAR
        [HttpPut("{id}")]
        public async Task<IActionResult> AlterarPessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            try
            {
                bool existeStatus = Enum.IsDefined(typeof(Pessoa.StatusPessoa), pessoa.Status);

                if (!existeStatus)
                {
                    return BadRequest(new { message = "O Status informado não é válido." });
                }

                _context.Entry(pessoa).State = EntityState.Modified;

                //*CRIA HISTORICO DE ALTERACAO
                HistoricoAlteracao historicoAlteracao = new HistoricoAlteracao();

                historicoAlteracao.Pessoa = pessoa;
                historicoAlteracao.IdPessoa = pessoa.Id;
                historicoAlteracao.Data = DateTime.Now;
                historicoAlteracao.Status = pessoa.Status;
                historicoAlteracao.Usuario = pessoa.Usuario;//*AQUI NÃO CONSEGUI ENTENDER MUITO BEM POIS NAO TEMOS AUTENTICACAO PARA SALVA O USUARIO LOGADO, ENTAO SALVEI O NOME DE USUARIO DA PESSOA.

                _context.HistoricoAlteracaos.Add(historicoAlteracao);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PessoaExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //*SALVAR
        [HttpPost]
        public async Task<ActionResult<Pessoa>> SalvarPessoa(Pessoa pessoa)
        {
            var pessoaExistente = await _context.Pessoa.Where(x => x.Documento == pessoa.Documento
                                                            && x.Telefone == pessoa.Telefone).FirstOrDefaultAsync();

            if (pessoaExistente != null)
            {
                return BadRequest(new { message = "Já existe uma pessoa com as mesmas informações de Documento e Telefone já cadastrados" });
            }

            bool existeStatus = Enum.IsDefined(typeof(Pessoa.StatusPessoa), pessoa.Status);

            if (!existeStatus)
            {
                return BadRequest(new { message = "O Status informado não é válido." });
            }

            _context.Pessoa.Add(pessoa);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("BuscaPessoa", new { id = pessoa.Id }, pessoa);
        }

        //*EXCLUIR.
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirPessoa(int id)
        {
            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region OUTROS

        private bool PessoaExiste(int id)
        {
            return _context.Pessoa.Any(e => e.Id == id);
        }

        #endregion

    }
}
