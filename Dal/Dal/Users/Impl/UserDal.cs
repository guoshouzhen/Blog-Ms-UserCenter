using Dal.DalBases.Impl;
using Infrastructure.Autofac.Attributes;
using Infrastructure.Utils;
using Model.Entities;
using Model.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Dal.Users.Impl
{
    /// <summary>
    /// 用户
    /// </summary>
    [Repository]
    public class UserDal : DalBase, IUserDal
    {
        /// <summary>
        /// 根据用户名查询用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<User> SelectUserByUsernameAsync(string username)
        {
            //算出dbid
            string dbid = DbUtil.GetDbId(username);
            //切换数据库
            DalContext.SwitchDb(dbid);
            var result = await DalContext.UnitOfWork.Table<User>().GetByConditionsAsync(x => x.UserName == username && x.Status != (int)UserStatusEnum.DELETED);
            //重置为默认数据库
            DalContext.ResetDb();
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<User> InsertUserAsync(User user)
        {
            User newUser = null;
            if (user != null) 
            {
                var dbid = DbUtil.GetDbId(user.UserName);
                DalContext.SwitchDb(dbid);
                newUser = await DalContext.UnitOfWork.Table<User>().AddAsync(user);
                _ = await DalContext.UnitOfWork.SaveChangesAsync();
                DalContext.ResetDb();
            }
            return newUser;
        }
    }
}
