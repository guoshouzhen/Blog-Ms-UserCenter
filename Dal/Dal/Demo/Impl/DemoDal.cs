using Dal.DalBases.Impl;
using Infrastructure.Autofac.Attributes;
using Model.Entities;
using MySqlConnector;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Dal.Demo.Impl
{
    [Repository]
    public class DemoDal : DalBase, IDemoDal
    {
        public async Task<User> GetUserByIdAsync(long id)
        {
            var result = await DalContext.UnitOfWork.Table<User>()
                .GetByConditionsAsync(x => x.Id == id);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 原生sql测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetAuthoritiesOfUserAsync(long id)
        {
            string sql = @"SELECT u.id, u.username,r.`name`, r.`description`
                            FROM t_user AS u
                            JOIN t_user_role AS ur ON ur.`user_id` = u.`id`
                            JOIN t_role AS r ON r.`id` = ur.`role_id`
                            WHERE u.id = @userId; ";
            MySqlParameter[] mySqlParameters = new MySqlParameter[]
            {
                new MySqlParameter("@userId", MySqlDbType.Int64){Value = id }
            };

            var result = await Task.FromResult(DalContext.UnitOfWork.QueryBySql(sql, mySqlParameters));
            string desc = result?.Rows[0]["name"]?.ToString();
            return desc;
        }
    }
}
