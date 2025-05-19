namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITarefaRepository Tarefas { get; }
        
        int Commit();
    }

}
