using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VersionManagement.Models
{
    /// <summary>
    /// 产品部门列表
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Department
    {
        /// <summary>
        /// 所有BU
        /// </summary>
        All,

        /// <summary>
        /// 泛耘
        /// </summary>
        BPO,

        /// <summary>
        /// 泛员网
        /// </summary>
        FYU,

        /// <summary>
        /// 易社保
        /// </summary>
        ESB,

        /// <summary>
        /// 昊天
        /// </summary>
        HaoTian,

        /// <summary>
        /// 泛优
        /// </summary>
        FanYou
    }
}
