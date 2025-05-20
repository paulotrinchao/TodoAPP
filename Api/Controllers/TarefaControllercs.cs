using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _service;

        public TarefaController(ITarefaService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Atualizar tarefa.")]
        public async Task<IActionResult> Criar([FromBody] TarefaDto dto)
        {
            var tarefa = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Listar tarefa por Id.")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var tarefa = await _service.ObterPorIdAsync(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar tarefas com filtro opcional por status e data de vencimento.")]
        public async Task<IActionResult> Listar([FromQuery, SwaggerParameter("Status da tarefa: 0 - Pendente, 1 - Em Andamento, 2 - Concluido")] StatusTarefa? status, 
                                                [FromQuery, SwaggerParameter("Data de vencimento : yyyy-mm-dd")] DateTime? vencimento)
        {
            var tarefas = await _service.ListarAsync(status, vencimento);
            return Ok(tarefas);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar tarefa.")]
        public async Task<IActionResult> Atualizar(int id, TarefaDto dto)
        {
            await _service.AtualizarAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover tarefa por Id.")]
        public async Task<IActionResult> Remover(int id)
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
    }
}
