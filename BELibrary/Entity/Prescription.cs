namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prescription")]
    public partial class Prescription
    {
        public Guid Id { get; set; }

        public Guid DetailPrescriptionId { get; set; }

        public Guid DetailRecordId { get; set; }

        public virtual DetailPrescription DetailPrescription { get; set; }

        public virtual DetailRecord DetailRecord { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}