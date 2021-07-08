using TaskTracker.Enums;

namespace TaskTracker.Data.Dto
{
    public class TaskDtoUpdate
    {
        /// <summary>
        /// Identifier. Unique key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Task name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Task description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Task status
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Task priority
        /// </summary>
        public int Priority { get; set; }

    }
}
