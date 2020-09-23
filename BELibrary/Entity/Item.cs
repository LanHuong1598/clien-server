namespace BELibrary.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Item")]
    public partial class Item
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public int Amount { get; set; }

        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}