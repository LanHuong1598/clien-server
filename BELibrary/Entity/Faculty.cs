using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BELibrary.Entity
{
    [Table("Faculty")]
    public class Faculty
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}