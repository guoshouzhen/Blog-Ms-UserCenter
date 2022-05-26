using Model.Entities;
using System.Threading.Tasks;

namespace Service.Demo
{
    public interface IDemoService
    {
        Task<string> GetTestStringsAsync();

        Task<string> GetLogTestStringsAsync();

        Task<User> GetUserAsync();
    }
}
