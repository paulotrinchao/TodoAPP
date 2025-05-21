using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Repositories
{
    public class TarefaRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly TarefaRepository _repository;
        private readonly Tarefa _entityMoq;
        private readonly Tarefa[] _tarefasMoq;

        public TarefaRepositoryTests()
        {

            _entityMoq = CriarTarefa(1, "Tarefa 1", "Descricao 1", StatusTarefa.EmAndamento, DateTime.Now);
            _tarefasMoq = CriarTarefas();

            var loggerMock = new Mock<ILogger<AppDbContext>>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new AppDbContext(options,loggerMock.Object);
            _repository = new TarefaRepository(_context);
        }

        private static Tarefa CriarTarefa(int id, string titulo, string descricao, StatusTarefa status, DateTime dataVencimento)
        {
            return new Tarefa
            {
                Id = id,
                Titulo = titulo,
                Descricao = descricao,
                Status = status,
                DataVencimento = dataVencimento
            };
        }

        private static Tarefa[] CriarTarefas()
        {

            var resultado = new[]
                 {
                    CriarTarefa(1, "Tarefa 1", "Descricao 1", StatusTarefa.EmAndamento, DateTime.Now),
                    CriarTarefa(2, "Tarefa 2", "Descricao 2", StatusTarefa.Concluido, DateTime.Now.Date)
                 };

            return resultado;

        }



        [Fact]
        public async Task GetByIdAsync_Calls_FindAsync()
        {

            await _context.Tarefas.AddAsync(_entityMoq);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(_entityMoq.Id, result.Id);
            Assert.Equal(_entityMoq.Titulo, result.Titulo);
            Assert.Equal(_entityMoq.Descricao, result.Descricao);
            Assert.Equal(_entityMoq.Descricao, result.Descricao);
            Assert.Equal(_entityMoq?.DataVencimento.Value, result.DataVencimento.Value);

        }

        [Fact]
        public async Task GetAllAsync_Returns_AllEntities()
        {
            
            await _context.Tarefas.AddRangeAsync(_tarefasMoq);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task FindAsync_Returns_FilteredEntities()
        {
            
            await _context.Tarefas.AddRangeAsync(_tarefasMoq);
            await _context.SaveChangesAsync();

            var result = await _repository.FindAsync(t => t.Titulo == _entityMoq.Titulo);

            Assert.NotNull(result);
            Assert.Equal(_entityMoq.Titulo, result.First().Titulo);
        }

        [Fact]
        public async Task AddAsync_Calls_AddAsync()
        {
            

            await _repository.AddAsync(_entityMoq);
            await _repository.SaveChangesAsync();

            var result = await _context.Tarefas.FindAsync(1);

            Assert.NotNull(result);
            Assert.Equal(_entityMoq.Titulo, result.Titulo);
            Assert.Equal(_entityMoq.Descricao, result.Descricao);
            Assert.Equal(_entityMoq.Descricao, result.Descricao);
            Assert.Equal(_entityMoq?.DataVencimento.Value, result.DataVencimento.Value);
        }

        [Fact]
        public async Task UpdateAsync_Calls_Update()
        {
            string novoTitulo = "Titulo Atualizado";

            await _context.Tarefas.AddAsync(_entityMoq);
            await _context.SaveChangesAsync();

            
            _context.Entry(_entityMoq).State = EntityState.Detached;

            var entityUpdate = await _context.Tarefas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == 1);


            entityUpdate.Titulo = novoTitulo;
            await _repository.UpdateAsync(entityUpdate);
            await _repository.SaveChangesAsync();

            var updated = await _context.Tarefas.FindAsync(1);

            Assert.Equal(novoTitulo, updated.Titulo);
            Assert.NotEqual(_entityMoq.Titulo, updated.Titulo);
        }

        [Fact]
        public async Task DeleteAsync_Calls_Remove()
        {

            await _context.Tarefas.AddAsync(_entityMoq);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(_entityMoq);
            await _repository.SaveChangesAsync();

            var result = await _context.Tarefas.FindAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task SaveChangesAsync_Calls_SaveChangesAsync()
        {
            var entity = CriarTarefa(1, "Salvar", "Descricao", StatusTarefa.EmAndamento, DateTime.Now);

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            var result = await _context.Tarefas.FirstOrDefaultAsync();

            Assert.NotNull(result);
            Assert.Equal("Salvar", result.Titulo);
        }
    }
}
