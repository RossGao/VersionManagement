using System;
using System.Collections.Generic;
using VersionManagement.Models;

namespace VersionManagement.Dtos
{
    /// <summary>
    /// 分页结果
    /// </summary>
    public class PageDto
    {
        // 总记录数
        public int TotalItems { get; set; }

        // 总页数
        public int TotalPages { get; set; }

        // 每页的记录数
        public int limit { get; set; }

        // 页码
        public int pageIndex { get; set; }

        ///// <summary>
        ///// 相关页面URI路径
        ///// </summary>
        //public Links Links { get; set; }

        /// <summary>
        /// 获取或设置当前页码的记录
        /// </summary>
        public dynamic Items { get; set; }
    }

    /// <summary>
    /// 页面链接
    /// </summary>
    public class Links
    {
        /// <summary>
        /// 前一页
        /// </summary>
        public Uri Previous { get; set; }

        /// <summary>
        /// 下一页
        /// </summary>
        public Uri Next { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public Uri Self { get; set; }
    }

    /// <summary>
    /// 版本详情分页信息
    /// </summary>
    public class VersionInfoPageDto : PageDto
    {
        public new ICollection<VersionInfo> Items { get; set; }
    }
}
