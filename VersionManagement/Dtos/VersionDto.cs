using System;
using System.ComponentModel.DataAnnotations;

namespace VersionManagement.Dtos
{
    public class VersionDto
    {
        /// <summary>
        /// 版本GUID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 版本上线时间
        /// </summary>
        [Required]
        public DateTime? ReleaseDate { get; set; }

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
        /// 产品部门. 0:所有;1:泛耘;2:泛员网;3:易社保;4:昊天;5:泛优
        /// </summary>
        [Required]
        public string Department { get; set; }

        /// <summary>
        /// 版本创建人姓名
        /// </summary>
        [Required]
        public string Creator { get; set; }

        /// <summary>
        /// 版本状态. 0:所有状态;1:已审核;2:未审核
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 版本发布详细描述
        /// </summary>
        public string ReleaseNote { get; set; }
    }
}
