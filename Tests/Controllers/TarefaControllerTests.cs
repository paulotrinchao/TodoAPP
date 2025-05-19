using Api.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Controllers
{
    public class TarefaControllerTests
    {
        private readonly Mock<ITarefaService> _serviceMock;
        private readonly TarefaController _controller;

        public TarefaControllerTests()
        {
            _serviceMock = new Mock<ITarefaService>();
            _controller = new TarefaController(_serviceMock.Object);
        }

        private TarefaDto GetDto() => new("Titulo Teste", "Descricao Teste", StatusTarefa.EmAndamento, DateTime.Today);
        private Tarefa GetTarefa(int id = 1) => new()
        {
            Id = id,
            Titulo = "Titulo",
            Descricao = "Descricao",
            DataVencimento = DateTime.Today,
            Status = StatusTarefa.Pendente
        };

        [Fact]
        public async Task Criar_DeveRetornarCreatedAtAction()
        {
            var dto = GetDto();
            var tarefa = GetTarefa();
            _serviceMock.Setup(s => s.CriarAsync(dto)).ReturnsAsync(tarefa);

            var result = await _controller.Criar(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.ObterPorId), created.ActionName);
            Assert.Equal(tarefa, created.Value);
        }

        [Fact]
        public async Task ObterPorId_TarefaExistente_DeveRetornarOk()
        {
            var tarefa = GetTarefa();
            _serviceMock.Setup(s => s.ObterPorIdAsync(tarefa.Id)).ReturnsAsync(tarefa);

            var result = await _controller.ObterPorId(tarefa.Id);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tarefa, ok.Value);
        }

        [Fact]
        public async Task ObterPorId_TarefaNaoEncontrada_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.ObterPorIdAsync(99)).ReturnsAsync((Tarefa)null!);

            var result = await _controller.ObterPorId(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Listar_DeveRetornarListaDeTarefas()
        {
            var tarefas = new List<Tarefa> { GetTarefa(1), GetTarefa(2) };
            _serviceMock.Setup(s => s.ListarAsync(null, null)).ReturnsAsync(tarefas);

            var result = await _controller.Listar(null, null);

            var ok = Assert.IsType<OkObjectResult>(result);
            var retorno = Assert.IsAssignableFrom<IEnumerable<Tarefa>>(ok.Value);
            Assert.Equal(2, retorno.Count());
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNoContent()
        {
            var dto = GetDto();

            var result = await _controller.Atualizar(1, dto);

            _serviceMock.Verify(s => s.AtualizarAsync(1, dto), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Remover_DeveRetornarNoContent()
        {
            var result = await _controller.Remover(1);

            _serviceMock.Verify(s => s.RemoverAsync(1), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
