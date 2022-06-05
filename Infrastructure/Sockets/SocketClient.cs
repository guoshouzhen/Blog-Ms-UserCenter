using Infrastructure.Autofac.Attributes;
using Infrastructure.Autofac.Enums;
using Infrastructure.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Infrastructure.Sockets
{
    /// <summary>
    /// socket客户端
    /// </summary>
    [Component(LifeCycle = ObjectLifeCycleEnum.SingleInstance)]
    public class SocketClient
    {
        /// <summary>
        /// 注入logger
        /// </summary>
        private readonly ILoggerHelper<SocketClient> _loggerHelper;

        public SocketClient(ILoggerHelper<SocketClient> loggerHelper)
        {
            _loggerHelper = loggerHelper;
        }


        /// <summary>
        /// 默认超时时间
        /// </summary>
        private static int TIMEOUT = 5 * 1000;

        /// <summary>
        /// 通过socket向远程主机请求数据
        /// </summary>
        /// <param name="strIp">主机ip</param>
        /// <param name="port">端口号</param>
        /// <param name="param">请求数据</param>
        /// <param name="timeout">接收超时时间</param>
        /// <returns>请求返回的数据</returns>
        public string GetSocketData(string strIp, int port, string param, int timeout)
        {
            string strResult = string.Empty;
            string socketInfo = strIp + ":" + port;
            // buffer for incoming data
            byte[] bytes = new byte[1024];
            Socket socket = null;
            string stepDesc = string.Empty;
            try
            {
                stepDesc = "step1--init...";
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(strIp), port);

                stepDesc = "step2--connecting...";
                socket.Connect(remoteEP);

                byte[] msg = Encoding.UTF8.GetBytes(param);
                //前四个字节必须是实际请求字节的总长度
                int len = msg.Length;
                if (len > int.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("socket请求数据的长度超出限制");
                }
                byte[] aryLen = new byte[4];
                // 请求数据的前4位是请求数据长度（字节序列大端方式存储）
                aryLen[0] = (byte)((len >> 24) & 0xff);
                aryLen[1] = (byte)((len >> 16) & 0xff);
                aryLen[2] = (byte)((len >> 8) & 0xff);
                aryLen[3] = (byte)(len & 0xff);

                byte[] aryRequestByte = new byte[4 + len];
                Array.Copy(aryLen, 0, aryRequestByte, 0, 4);
                Array.Copy(msg, 0, aryRequestByte, 4, len);

                stepDesc = "step3--sending...";
                int bytesSent = socket.Send(aryRequestByte);

                StringBuilder sbRecData = new StringBuilder();
                socket.ReceiveTimeout = timeout;
                stepDesc = "step4--receiving...";

                //先获取数据长度
                byte[] aryRecDataLen = new byte[4];
                socket.Receive(aryRecDataLen);

                long reDatacLen = (aryRecDataLen[0] << 24) & 0xff000000;
                reDatacLen += (aryRecDataLen[1] << 16) & 0xff0000;
                reDatacLen += (aryRecDataLen[2] << 8) & 0xff00;
                reDatacLen += aryRecDataLen[3] & 0xff;
                if (reDatacLen == 0)
                {
                    return string.Empty;
                }

                while (true)
                {
                    int bytesRec = socket.Receive(bytes, bytes.Length, SocketFlags.None);

                    sbRecData.Append(Encoding.UTF8.GetString(bytes, 0, bytesRec));
                    if (bytesRec < bytes.Length)
                    {
                        break;
                    }
                }

                strResult = sbRecData.ToString();
            }
            catch (Exception ex)
            {
                _loggerHelper.ErrorLog($"socket请求数据时发生异常，异常信息：{ex}，远程主机：{socketInfo}，请求参数：{param}，执行步骤：{stepDesc}");
            }
            finally
            {
                // Release the socket.  
                stepDesc = "step5--closing...";
                if (socket != null && socket.Connected)
                {
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    catch (Exception ex)
                    {
                        _loggerHelper.ErrorLog($"关闭socket连接时发生异常，异常信息：{ex}，远程主机：{socketInfo}，请求参数：{param}，执行步骤：{stepDesc}");
                    }
                }
            }
            return strResult;
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="strIp"></param>
        /// <param name="port"></param>
        /// <param name="param"></param>
        /// <returns>返回的数据</returns>
        public string GetSocketData(string strIp, int port, string param)
        {
            return GetSocketData(strIp, port, param, TIMEOUT);
        }
    }
}
