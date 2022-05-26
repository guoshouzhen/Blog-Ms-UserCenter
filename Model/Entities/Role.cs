using Model.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities
{
    [Table("t_role")]
    public class Role : Entity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }
        /// <summary>
        /// 角色名（权限名）
        /// </summary>
        [Column("name")]
        public int Name { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>
        [Column("description")]
        public int Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_date")]
        public DateTime? CreateTime { get; set; }
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
