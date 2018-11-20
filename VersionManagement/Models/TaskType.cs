namespace VersionManagement.Models
{
    /// <summary>
    /// 任务类型。0：所有;1：用户故事; 2 : bug
    /// </summary>
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
