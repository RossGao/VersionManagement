namespace VersionManagement.Models
{
    /// <summary>
    /// 版本发布状态列表
    /// </summary>
    public enum VersionStatus
    {
        /// <summary>
        /// 所有状态
        /// </summary>
        Undefined,

        /// <summary>
        /// 未审核
        /// </summary>
        unaudited,

        /// <summary>
        /// 已审核
        /// </summary>
        audited
    }
}
