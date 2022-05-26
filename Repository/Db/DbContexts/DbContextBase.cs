using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Entities;
using System;

namespace Repository.Db.DbContexts
{
    /// <summary>
    /// 数据库上下文自定义基类
    /// </summary>
    public abstract class DbContextBase : DbContext
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected abstract string ConnectionString { get; }

        #region 指定每个表的DbSet属性，供DbContext调用
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        #endregion

        /// <summary>
        /// 配置数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            var mysqlVerson = new MySqlServerVersion(new Version(5, 7, 30));

            optionsBuilder
                .UseMySql(ConnectionString, mysqlVerson)
                //TODO：只在debuge时，打印info日志
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}
