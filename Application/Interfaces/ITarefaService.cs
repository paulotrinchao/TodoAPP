using Application.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface ITarefaService
    {
        Task<IEnumerable<Tarefa>> ListarAsync(StatusTarefa? status, DateTime? vencimento);
        Task<Tarefa> ObterPorIdAsync(int id);
        Task<Tarefa> CriarAsync(TarefaDto dto);
        Task AtualizarAsync(int id, TarefaDto dto);
        Task RemoverAsync(int id);
    }
}
