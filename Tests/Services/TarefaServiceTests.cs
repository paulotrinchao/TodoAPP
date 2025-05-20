using System.Linq.Expressions;
using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;

namespace Tests.Services
{
    public class TarefaServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITarefaRepository> _tarefaRepoMock;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _tarefaRepoMock = new Mock<ITarefaRepository>();

            _unitOfWorkMock.Setup(u => u.Tarefas).Returns(_tarefaRepoMock.Object);

            _service = new TarefaService(_unitOfWorkMock.Object);
        }

        private TarefaDto GetDto() => new("Título", "Descrição", StatusTarefa.EmAndamento, DateTime.Today);

        private Tarefa GetTarefa(int id = 1) => new()
        {
            Id = id,
            Titulo = "Título",
            Descricao = "Descrição",
            Status = StatusTarefa.Pendente,
            DataVencimento = DateTime.Today
        };

        [Fact]
        public async Task CriarAsync_DeveCriarNovaTarefa()
        {
            var dto = GetDto();

            var result = await _service.CriarAsync(dto);

            _tarefaRepoMock.Verify(r => r.AddAsync(It.IsAny<Tarefa>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
            Assert.Equal(dto.Titulo, result.Titulo);
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarTodasAsTarefas()
        {
            var tarefas = new List<Tarefa> { GetTarefa(1), GetTarefa(2) };

            _tarefaRepoMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Tarefa, bool>>>()))
                .ReturnsAsync((Expression<Func<Tarefa, bool>> predicate) =>
                    tarefas.Where(predicate.Compile()).ToList());

            var result = await _service.ListarAsync(null, null);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task ListarAsync_ComFiltroStatus_DeveFiltrar()
        {
            var tarefas = new List<Tarefa>
            {
                new Tarefa { Status = StatusTarefa.Pendente },
                new Tarefa { Status = StatusTarefa.Concluido }
            };
            _tarefaRepoMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Tarefa, bool>>>()))
                .ReturnsAsync((Expression<Func<Tarefa, bool>> predicate) =>
                    tarefas.Where(predicate.Compile()).ToList());

            var result = await _service.ListarAsync(StatusTarefa.Concluido, null);

            Assert.Single(result);
            Assert.All(result, t => Assert.Equal(StatusTarefa.Concluido, t.Status));
        }

        [Fact]
        public async Task ListarAsync_ComFiltroDataVencimento_DeveFiltrar()
        {
            var data = DateTime.Today;
            var tarefas = new List<Tarefa>
            {
                new() { DataVencimento = data },
                new() { DataVencimento = data.AddDays(1) }
            };

            _tarefaRepoMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Tarefa, bool>>>()))
                .ReturnsAsync((Expression<Func<Tarefa, bool>> predicate) =>
                    tarefas.Where(predicate.Compile()).ToList());

            var result = await _service.ListarAsync(null, data);

            Assert.Single(result);
            Assert.All(result, t => Assert.Equal(data, t.DataVencimento));
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarTarefa()
        {
            var tarefa = GetTarefa();
            _tarefaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tarefa);

            var result = await _service.ObterPorIdAsync(1);

            Assert.Equal(tarefa.Id, result.Id);
        }

        [Fact]
        public async Task AtualizarAsync_ComTarefaExistente_DeveAtualizar()
        {
            var tarefa = GetTarefa();
            var dto = GetDto();
            _tarefaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tarefa);

            await _service.AtualizarAsync(1, dto);

            _tarefaRepoMock.Verify(r => r.UpdateAsync(It.Is<Tarefa>(t => t.Titulo == dto.Titulo)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task AtualizarAsync_TarefaNaoEncontrada_NaoAtualiza()
        {
            _tarefaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Tarefa)null!);

            await _service.AtualizarAsync(1, GetDto());

            _tarefaRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Tarefa>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }

        [Fact]
        public async Task RemoverAsync_ComTarefaExistente_DeveRemover()
        {
            var tarefa = GetTarefa();
            _tarefaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tarefa);

            await _service.RemoverAsync(1);

            _tarefaRepoMock.Verify(r => r.DeleteAsync(tarefa), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_TarefaNaoEncontrada_NaoRemove()
        {
            _tarefaRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Tarefa)null!);

            await _service.RemoverAsync(1);

            _tarefaRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Tarefa>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Never);
        }
    }
}
