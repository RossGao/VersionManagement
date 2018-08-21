using System;
using System.ComponentModel.DataAnnotations;

namespace VersionManagement.Models
{
    /// <summary>
    /// 版本详情信息
    /// </summary>
    public class VersionDetail
    {
        /// <summary>
        /// 详情ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Required]
        public string TaskTitle { get; set; }

        /// <summary>
        /// 任务类型, 0：用户故事, 1:bug
        /// </summary>
        [Required]
        public TaskType Type { get; set; }

        /// <summary>
        /// 迭代周期
        /// </summary>
        [Required]
        public string Iteration { get; set; }

        /// <summary>
        /// 任务对应代码提交ID列表，以逗号分隔, 例：efcf915e,e9148f99,e740b46f
        /// </summary>
        [Required]
        public string CommitIds { get; set; }

        /// <summary>
        /// 修改内容详情
        /// </summary>
        public string DetailNote { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [Required]
        public string Applicant { get; set; }
    }
}
