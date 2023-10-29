namespace WebEvent.API.Repository.Abstract
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }

        Task Save();
    }
}
