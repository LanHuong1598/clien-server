namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Patient")]
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            Accounts = new HashSet<Account>();
            MedicalSupplies = new HashSet<MedicalSupply>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        public bool Gender { get; set; }

        [Required]
        [StringLength(12)]
        public string IndentificationCardId { get; set; }

        [Required]
        [StringLength(12)]
        public string Phone { get; set; }

        public Guid? RecordId { get; set; }

        public bool Status { get; set; }

        public string ImageProfile { get; set; }

        [Required]
        public string PatientCode { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime JoinDate { get; set; }

        public DateTime IndentificationCardDate { get; set; }

        public string Job { get; set; }

        public string WorkPlace { get; set; }

        public string HistoryOfIllnessFamily { get; set; }

        public string HistoryOfIllnessYourself { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalSupply> MedicalSupplies { get; set; }

        public virtual Record Record { get; set; }
    }
}
