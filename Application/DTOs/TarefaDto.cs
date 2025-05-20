using Domain.Enums;

namespace Application.DTOs
{
    public record TarefaDto(
    string Titulo,
    string Descricao,
    StatusTarefa Status,
    DateTime DataVencimento);
  
}
