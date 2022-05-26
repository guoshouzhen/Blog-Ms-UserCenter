using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Options
{
    /// <summary>
    /// socket远程服务配置
    /// </summary>
    public class SocketServerOptions
    {
        /// <summary>
        /// id服务
        /// </summary>
        public SocketAddrConfiguration IdServer { get; set; }
    }

    /// <summary>
    /// 节点配置
    /// </summary>
    public class SocketAddrConfiguration
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
    }
}
