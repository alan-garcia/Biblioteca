namespace BibliotecaNET8.Domain.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task<int> Save();
    }
}
