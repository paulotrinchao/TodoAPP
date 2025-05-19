using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly IUnitOfWork _repository;

        public TarefaService(IUnitOfWork repo) => _repository = repo;

        public async Task<Tarefa> CriarAsync(TarefaDto dto)
        {
            var tarefa = new Tarefa
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                DataVencimento = dto.DataVencimento,
                Status = StatusTarefa.Pendente
            };

            await _repository.Tarefas.AddAsync(tarefa);
            _repository.Commit();
            return tarefa;
        }
        public async Task<IEnumerable<Tarefa>> ListarAsync(StatusTarefa? status, DateTime? vencimento)
        {
            var tarefas = await _repository.Tarefas.GetAllAsync();

            if (status.HasValue)
                tarefas = tarefas.Where(t => t.Status == status);

            if (vencimento.HasValue)
                tarefas = tarefas.Where(t => t?.DataVencimento?.Date == vencimento.Value.Date);

            return tarefas;
        }

        public async Task<Tarefa?> ObterPorIdAsync(int id) =>
             await _repository.Tarefas.GetByIdAsync(id);


        public async Task AtualizarAsync(int id, TarefaDto dto)
        {
            var tarefa = await _repository.Tarefas.GetByIdAsync(id);
            if (tarefa == null) return;

            tarefa.Titulo = dto.Titulo;
            tarefa.Descricao = dto.Descricao;
            tarefa.DataVencimento = dto.DataVencimento;
            tarefa.Status = dto.Status;


            await _repository.Tarefas.UpdateAsync(tarefa);
            _repository.Commit();
        }

        public async Task RemoverAsync(int id)
        {
            var tarefa = await _repository.Tarefas.GetByIdAsync(id);
            if (tarefa == null) return;

            await _repository.Tarefas.DeleteAsync(tarefa);
            _repository.Commit();
        }

    }
}
