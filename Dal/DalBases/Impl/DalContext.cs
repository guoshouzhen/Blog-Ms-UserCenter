using Infrastructure.Autofac.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Model.Options;
using Repository.Db.DbUnitOfWork;
using Repository.Db.DbUnitOfWork.UnitPartial;
using System;

namespace Dal.DalBases.Impl
{
    /// <summary>
    /// Dal上下文实现
    /// </summary>
    [Repository]
    public class DalContext : IDalContext
    {
        /// <summary>
        /// 注入IServiceProvider容器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// 注入数据库id配置
        /// </summary>
        private readonly DbIdConfig _dbIdConfig;
        
        /// <summary>
        /// 根据注入的数据库id创建工作单元
        /// </summary>
        private UnitOfWorkBase _unitOfWork;

        public DalContext(IServiceProvider serviceProvider,DbIdConfig dbIdConfig)
        {
            _serviceProvider = serviceProvider;
            _dbIdConfig = dbIdConfig;
        }

        /// <summary>
        /// 根据当前请求设置的全局dbid返回对应数据库工作单元
        /// </summary>
        public UnitOfWorkBase UnitOfWork
        {
            get 
            {
                if (_unitOfWork == null) 
                {
                    _unitOfWork = CreateUnitOfWork(_dbIdConfig.DbId);
                }
                return _unitOfWork;
            }
        }

        /// <summary>
        /// 切换数据库
        /// 此处使用完需要手动切回当前请求域的默认数据库
        /// </summary>
        /// <param name="strDbId"></param>
        /// <returns></returns>
        public IDalContext SwitchDb(string strDbId)
        {
            _unitOfWork = CreateUnitOfWork(strDbId);
            return this;
        }

        /// <summary>
        /// 重置数据库为当前请求域数据库
        /// </summary>
        /// <returns></returns>
        public IDalContext ResetDb()
        {
            _unitOfWork = null;
            return this;
        }

        /// <summary>
        /// 根据dbid创建数据库工作单元
        /// </summary>
        /// <param name="strDbId"></param>
        /// <returns></returns>
        private UnitOfWorkBase CreateUnitOfWork(string strDbId) 
        {
            //默认主库（1库）
            UnitOfWorkBase unitOfWorkBase = _serviceProvider.GetRequiredService<Blog1UnitOfWork>();
            switch (strDbId) 
            {
                case "1":
                    unitOfWorkBase = _serviceProvider.GetRequiredService<Blog1UnitOfWork>();
                    break;
                case "2":
                    unitOfWorkBase = _serviceProvider.GetRequiredService<Blog2UnitOfWork>();
                    break;
                default:
                    break;
            }
            return unitOfWorkBase;
        }
    }
}
