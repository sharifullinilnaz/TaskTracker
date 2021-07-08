using TaskTracker.Enums;

namespace TaskTracker.Data.Dto
{
    /// <summary>
    /// Project
    /// </summary>
    public class ProjectDtoUpdate
    {
        /// <summary>
        /// Identifier. Unique key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Project status
        /// </summary>
        public ProjectStatus Status { get; set; }

        /// <summary>
        /// Project priority
        /// </summary>
        public int Priority { get; set; }

    }
}
