using System;
using System.Collections.Generic;

namespace TaskTracker.Data.Dto
{
    /// <summary>
    /// Project
    /// </summary>
    public class ProjectDtoResponse
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
        /// Project start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Project completion date
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// Project status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Project priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Projecet tasks.
        /// </summary>
        public IEnumerable<TaskDtoResponse> Tasks { get; set; }

    }
}
