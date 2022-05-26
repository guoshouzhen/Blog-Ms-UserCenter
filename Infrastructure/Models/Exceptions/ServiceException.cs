using Model.Constant;
using Model.Vo;
using System;

namespace Infrastructure.Exceptions
{
    /// <summary>
    /// 自定义业务异常
    /// </summary>
    public class ServiceException : Exception
    {
        public string Code { get; private set; }
        public string Msg { get; private set; }

        public ServiceException() 
            : base() 
        {
            Code = ErrorCodeVo.A0000.Code;
            Msg = ErrorCodeVo.A0000.Message;
        }

        public ServiceException(string msg)
            : base(msg)
        {
            Code = ApiResultFailedConstant.CODE;
            Msg = msg;
        }

        /// <summary>
        /// 业务异常暂不记录栈追踪信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public ServiceException(string msg, Exception innerException)
            : base(msg, innerException) 
        {
            Code = ApiResultFailedConstant.CODE;
            Msg = msg;
        }

        public ServiceException(string code, string msg)
        {
            Code = code;
            Msg = msg;
        }

        public ServiceException(ErrorCodeVo errorCodeVo)
           : this(errorCodeVo.Code, errorCodeVo.Message) { }
    }
}
