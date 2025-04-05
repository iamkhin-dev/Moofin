using System.Threading.Tasks;

namespace Moofin.Core.Interfaces
{
    public interface IStorage
    {
        Task SaveAsync<T>(string key, T data);
        Task<T> LoadAsync<T>(string key);
        Task DeleteAsync(string key);
    }
}