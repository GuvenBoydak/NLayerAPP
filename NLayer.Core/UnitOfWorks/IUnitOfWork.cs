namespace NLayer.Core
{
    public interface IUnitOfWork
    {
        Task CommitAsync();

        void Commit();
    }
}
