using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SocketApi
{
    /// <summary>
    /// 通过socket获取数据
    /// </summary>
    public interface ISocketService
    {
        /// <summary>
        /// 申请用户id
        /// </summary>
        /// <returns></returns>
        long GetUserId();
    }
}
