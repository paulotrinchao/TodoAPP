using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ITarefaRepository Tarefas { get; }
        public UnitOfWork(AppDbContext context, ITarefaRepository tarefaRepository)
        {
            _context = context;
            this.Tarefas = tarefaRepository;
        }
        
        public int Commit()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
