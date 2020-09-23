using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BELibrary.Entity
{
    [Table("Doctor")]
    public class Doctor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool Gender { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Guid FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }
    }
}