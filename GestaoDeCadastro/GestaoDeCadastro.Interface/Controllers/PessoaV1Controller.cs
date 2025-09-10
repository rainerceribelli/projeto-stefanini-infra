using GestaoDeCadastro.Crosscutting.DTO.Cadastro;
using GestaoDeCadastro.Crosscutting.DTO.Cadastro.V1;
using GestaoDeCadastro.Domain.Exceptions;
using GestaoDeCadastro.Services.ApplicationServices.Cadastro;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GestaoDeCadastro.Interface.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "teste,admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Pessoa")]
    [ApiController]
    public class PessoaV1Controller : ControllerBase
    {
        private readonly PessoaApplicationServices _service;

        public PessoaV1Controller(PessoaApplicationServices service)
        {
            _service = service;
        }

        [HttpGet("GetListPessoas")]
        public async Task<ActionResult<List<PessoaDTO>>> GetListPessoas()
        {
            try
            {
                var Pessoas = await _service.GetListPessoas();

                return Ok(Pessoas);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("CreatePessoa")]
        public async Task<ActionResult> CreatePessoa(CreatePessoaDTO CreatePessoa)
        {
            try
            {
                await _service.CreatePessoa(CreatePessoa);
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
        public async Task<ActionResult> UpdatePessoa(UpdatePessoaDTO UpdatePessoa)
        {
            try
            {
                await _service.UpdatePessoa(UpdatePessoa);
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
