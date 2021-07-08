using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TaskTracker.Enums;

namespace TaskTracker.Data.Models
{
    /// <summary>
    /// Task
    /// </summary>
    [Table("tasks")]
    public class Task
    {
        /// <summary>
        /// Identifier. Unique key
        /// </summary>
        [Column("id", TypeName = "serial")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Task name
        /// </summary>
        [Column("name", TypeName = "varchar(128)")]
        public string Name { get; set; }

        /// <summary>
        /// Task description
        /// </summary>
        [Column("description", TypeName = "varchar(256)")]
        public string Description { get; set; }

        /// <summary>
        /// Task status
        /// </summary>
        [Column("status", TypeName = "varchar(16)")]
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Task priority
        /// </summary>
        [Column("priority", TypeName = "integer")]
        public int Priority { get; set; }

        [ForeignKey("Project")]
        [Column("project_id", TypeName = "integer")]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }

    }
}
