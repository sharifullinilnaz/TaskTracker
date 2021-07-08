using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TaskTracker.Enums;

namespace TaskTracker.Data.Models
{
    /// <summary>
    /// Project
    /// </summary>
    [Table("projects")]
    public class Project
    {
        /// <summary>
        /// Identifier. Unique key
        /// </summary>
        [Column("id", TypeName = "serial")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Project name
        /// </summary>
        [Column("name", TypeName = "varchar(128)")]
        public string Name { get; set; }

        /// <summary>
        /// Project start date
        /// </summary>
        [Column("start_date", TypeName = "date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Project completion date
        /// </summary>
        [Column("completion_date", TypeName = "date")]
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// Project status
        /// </summary>
        [Column("status", TypeName = "varchar(16)")]
        public ProjectStatus Status { get; set; }

        /// <summary>
        /// Project priority
        /// </summary>
        [Column("priority", TypeName = "integer")]
        public int Priority { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }

    }
}
