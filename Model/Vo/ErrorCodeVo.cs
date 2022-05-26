namespace Model.Vo
{
    /// <summary>
    /// 错误code枚举类，可配合自定义异常类使用
    /// </summary>
    public class ErrorCodeVo
    {
        #region 字段定义
        /// <summary>
        /// 错误码
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// 错误信息（提示给前端的友好信息）
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 不允许从外部实例化
        /// </summary>
        private ErrorCodeVo() { }

        public ErrorCodeVo(string code, string msg)
        {
            Code = code;
            Message = msg;
        }
        #endregion

        #region 通用错误码定义（A开头）
        /// <summary>
        /// 提示：系统繁忙，请稍后重试
        /// </summary>
        public static readonly ErrorCodeVo A0000 = new ErrorCodeVo("A0000", "系统繁忙，请稍后重试");

        /// <summary>
        /// 提示：请求参数错误，请稍后重试
        /// </summary>
        public static readonly ErrorCodeVo A0001 = new ErrorCodeVo("A0001", "请求参数错误，请稍后重试");

        /// <summary>
        /// 提示：时间戳无效
        /// </summary>
        public static readonly ErrorCodeVo A0002 = new ErrorCodeVo("A0002", "时间戳无效");

        /// <summary>
        /// 提示：请求已过期
        /// </summary>
        public static readonly ErrorCodeVo A0003 = new ErrorCodeVo("A0003", "请求已过期");

        /// <summary>
        /// 提示：加密签名错误
        /// </summary>
        public static readonly ErrorCodeVo A0004 = new ErrorCodeVo("A0004", "加密签名错误");

        /// <summary>
        /// 提示：无效的token，请重新登录
        /// </summary>
        public static readonly ErrorCodeVo A0005 = new ErrorCodeVo("A0005", "无效的token，请重新登录");

        /// <summary>
        /// 提示：登录信息已过期，请重新登录
        /// </summary>
        public static readonly ErrorCodeVo A0006 = new ErrorCodeVo("A0006", "登录信息已过期，请重新登录");

        /// <summary>
        /// 提示：权限不足
        /// </summary>
        public static readonly ErrorCodeVo A0007 = new ErrorCodeVo("A0007", "权限不足");

        /// <summary>
        /// 提示：未获取到登录信息，请重新登录
        /// </summary>
        public static readonly ErrorCodeVo A0008 = new ErrorCodeVo("A0008", "未获取到登录信息，请重新登录");
        #endregion

        #region 业务错误码定义（用户中心U开头）
        /// <summary>
        /// 提示：用户不存在
        /// </summary>
        public static readonly ErrorCodeVo U0001 = new ErrorCodeVo("U0001", "用户不存在");
        /// <summary>
        /// 提示：用户名或密码错误
        /// </summary>
        public static readonly ErrorCodeVo U0002 = new ErrorCodeVo("U0002", "用户名或密码错误");
        /// <summary>
        /// 提示：该账户已被禁用，请联系管理员
        /// </summary>
        public static readonly ErrorCodeVo U0003 = new ErrorCodeVo("U0003", "该账户已被禁用，请联系管理员");



        /// <summary>
        /// 提示：用户名或密码不能为空
        /// </summary>
        public static readonly ErrorCodeVo U0100 = new ErrorCodeVo("U0100", "用户名或密码不能为空");
        /// <summary>
        /// 提示：用户名或密码错误
        /// </summary>
        public static readonly ErrorCodeVo U0101 = new ErrorCodeVo("U0101", "用户名或密码错误");
        /// <summary>
        /// 提示：登录失败，请稍后重试
        /// </summary>
        public static readonly ErrorCodeVo U0102 = new ErrorCodeVo("U0102", "登录失败，请稍后重试");
        /// <summary>
        /// 提示：用户名不能为空
        /// </summary>
        public static readonly ErrorCodeVo U0103 = new ErrorCodeVo("U0103", "用户名不能为空");
        /// <summary>
        /// 提示：用户名已被使用
        /// </summary>
        public static readonly ErrorCodeVo U0104 = new ErrorCodeVo("U0104", "用户名已被使用");
        /// <summary>
        /// 提示：密码不能为空
        /// </summary>
        public static readonly ErrorCodeVo U0105 = new ErrorCodeVo("U0105", "密码不能为空");
        /// <summary>
        /// 提示：密码长度必须为6位以上
        /// </summary>
        public static readonly ErrorCodeVo U0106 = new ErrorCodeVo("U0106", "密码长度必须为6位以上");
        /// <summary>
        /// 提示：邮箱不能为空
        /// </summary>
        public static readonly ErrorCodeVo U0107 = new ErrorCodeVo("U0107", "邮箱不能为空");
        /// <summary>
        /// 提示：邮箱格式错误
        /// </summary>
        public static readonly ErrorCodeVo U0108 = new ErrorCodeVo("U0108", "邮箱格式错误");
        /// <summary>
        /// 提示：
        /// </summary>
        public static readonly ErrorCodeVo U0109 = new ErrorCodeVo("U0109", "");
        /// <summary>
        /// 提示：手机号格式错误
        /// </summary>
        public static readonly ErrorCodeVo U0110 = new ErrorCodeVo("U0110", "手机号格式错误");

        /// <summary>
        /// 提示：用户注册失败，请稍后重试
        /// </summary>
        public static readonly ErrorCodeVo U0111 = new ErrorCodeVo("U0111", "用户注册失败，请稍后重试");


        #endregion

    }
}
