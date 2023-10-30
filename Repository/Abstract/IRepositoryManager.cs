namespace WebEvent.API.Repository.Abstract
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        IEventRepository Events{ get; }
        Task Save();
    }
}
