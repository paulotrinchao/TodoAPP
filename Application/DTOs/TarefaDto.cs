using Domain.Enums;

namespace Application.DTOs
{
    public record TarefaDto(
    string Titulo,
    string Descricao,
    StatusTarefa Status,
    DateTime DataVencimento);

    //public class TarefaDto
    //{
    //    public string Titulo { get; set; }
    //    public string Descricao { get; set; }
    //    public StatusTarefa Status { get; set; }
    //    public DateTime DataVencimento { get; set; }
    //}
}
