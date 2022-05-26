using Dal.DalBases;
using Model.Entities;
using System.Threading.Tasks;

namespace Dal.Dal.Demo
{
    public interface IDemoDal : IDalBase
    {
        Task<User> GetUserByIdAsync(long id);
        Task<string> GetAuthoritiesOfUserAsync(long id);
    }
}
