using Dal.DalBases;
using Model.Entities;
using System.Threading.Tasks;

namespace Dal.Dal.Users
{
    public interface IUserDal : IDalBase
    {
        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User> SelectUserByUsernameAsync(string username);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> InsertUserAsync(User user);
    }
}
