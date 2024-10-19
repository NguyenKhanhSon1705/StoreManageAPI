namespace StoreManageAPI.Services.Interfaces
{
    public interface IGenericCRUD<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
