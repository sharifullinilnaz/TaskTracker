using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Data.Dto
{
    /// <summary>
    /// Project
    /// </summary>
    public class ProjectDtoRequest
    {
        /// <summary>
        /// Project name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Project priority
        /// </summary>
        [Required]
        public int Priority { get; set; }

    }
}