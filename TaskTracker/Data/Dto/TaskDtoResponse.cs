namespace TaskTracker.Data.Dto
{
    public class TaskDtoResponse
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
        public string Status { get; set; }

        /// <summary>
        /// Task priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Task project id
        /// </summary>
        public int ProjectId { get; set; }

    }
}
