//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskManagementSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attachments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Attachments()
        {
            this.Results = new HashSet<Results>();
        }
    
        public int ID { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; }
        public string ServerFileName { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Tasks Tasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Results> Results { get; set; }
    }
}
