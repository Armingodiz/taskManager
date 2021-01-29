namespace taskManager.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("assignment")]
    public partial class assignment
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string taskName { get; set; }

        public int userId { get; set; }

        public virtual task task { get; set; }

        public virtual user user { get; set; }
    }
}
