using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VersionManagement.Models
{
    /// <summary>
    /// 版本信息
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// 版本GUID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 版本上线时间
        /// </summary>
        [Required]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// 版本号 (例:v1.2.6)
        /// </summary>
        [Required]
        public string VersionNumber { get; set; }

        /// <summary>
        /// 版本标题
        /// </summary>
        [Required]
        public string VersionTitle { get; set; }

        /// <summary>
        /// 产品部门
        /// </summary>
        [Required]
        public Department Department { get; set; }

        /// <summary>
        /// 版本创建人姓名
        /// </summary>
        [Required]
        public string Creator { get; set; }

        /// <summary>
        /// 版本状态
        /// </summary>
        public VersionStatus Status { get; set; }

        /// <summary>
        /// 版本发布详细描述
        /// </summary>
        public string ReleaseNote { get; set; }

        /// <summary>
        /// 版本详情列表
        /// </summary>
        public virtual ICollection<VersionDetail> Detailes { get; set; }
    }
}
