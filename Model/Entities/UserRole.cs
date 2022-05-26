using Model.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities
{
    [Table("t_user_role")]
    public class UserRole : Entity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Column("user_id")]
        public long UserId { get; set; }
        /// <summary>
        /// 角色id
        /// </summary>
        [Column("role_id")]
        public int RoleId { get; set; }
        /// <summary>
        /// 状态（0有效 1无效）
        /// </summary>
        [Column("status")]
        public int Status { get; set; }
    }
}
