using Model.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities
{
    [Table("t_user")]
    public class User : Entity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }
        /// <summary>
        /// 用户名（昵称），全局唯一
        /// </summary>
        [Column("username")]
        public string UserName { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Column("fullname")]
        public string FullName { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        [Column("email")]
        public string Email { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        [Column("mobile")]
        public string Mobile { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [Column("password")]
        public string PassWord { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        [Column("signature")]
        public string Signature { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        [Column("avatar")]
        public string Avatar { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("modify_date")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 状态 0有效 1无效
        /// </summary>
        [Column("status")]
        public int Status { get; set; }
    }
}
