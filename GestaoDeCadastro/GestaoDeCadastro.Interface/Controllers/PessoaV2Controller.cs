using GestaoDeCadastro.Crosscutting.DTO.Cadastro.V2;
using GestaoDeCadastro.Domain.Exceptions;
using GestaoDeCadastro.Services.ApplicationServices.Cadastro;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoDeCadastro.Interface.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "teste,admin")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/PessoaV2")]
    [ApiController]
    public class PessoaV2Controller : ControllerBase
    {
        private readonly PessoaApplicationServices _service;

        public PessoaV2Controller(PessoaApplicationServices service)
        {
            _service = service;
        }

        [HttpGet("GetListPessoas")]
        public async Task<ActionResult<List<PessoaV2DTO>>> GetListPessoas()
        {
            try
            {
                var Pessoas = await _service.GetListPessoasV2();

                return Ok(Pessoas);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("CreatePessoa")]
        public async Task<ActionResult> CreatePessoa(CreatePessoaV2DTO CreatePessoa)
        {
            try
            {
                await _service.CreatePessoaV2(CreatePessoa);

                return Ok("Pessoa criada com sucesso");
            }
            catch (DomainException ex)
            {
                return BadRequest(new { erros = ex.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erros = new[] { ex.Message } });
            }
        }

        [HttpPut("UpdatePessoa")]
        public async Task<ActionResult> UpdatePessoa(UpdatePessoaV2DTO UpdatePessoa)
        {
            try
            {
                await _service.UpdatePessoaV2(UpdatePessoa);

                return Ok("Pessoa atualizada com sucesso");
            }
            catch (DomainException ex)
            {
                return BadRequest(new { erros = ex.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erros = new[] { ex.Message } });
            }
        }

        [HttpDelete("DeletePessoa/{id}")]
        public async Task<ActionResult> DeletePessoa(int id)
        {
            try
            {
                await _service.DeletePessoa(id);
                return Ok("Pessoa excluída com sucesso");
            }
            catch (DomainException ex)
            {
                return BadRequest(new { erros = ex.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erros = new[] { ex.Message } });
            }
        }

    }
}
