using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    [Table("Results")]
    public class Result : BaseEntity
    {
        [ForeignKey("Attachment")]
        public int? AttachmentId { get; set; }
        public string ResultText { get; set; }

        public virtual Attachment Attachment { get; set; }
    }
}