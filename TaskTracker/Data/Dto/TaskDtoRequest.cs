using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Data.Dto
{
    public class TaskDtoRequest
    {
        /// <summary>
        /// Task name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Task description
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Task priority
        /// </summary>
        [Required]
        public int Priority { get; set; }

        /// <summary>
        /// Task project id
        /// </summary>
        [Required]
        public int ProjectId { get; set; }
    }
}
