using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VersionManagement.Models
{
    /// <summary>
    /// 任务类型
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TaskType
    {
        /// <summary>
        /// 所有类型
        /// </summary>
        All,

        /// <summary>
        /// 用户故事
        /// </summary>
        UserStory,

        /// <summary>
        /// Bug
        /// </summary>
        Bug,
    }
}
