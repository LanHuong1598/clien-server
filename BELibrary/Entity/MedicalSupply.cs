namespace BELibrary.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MedicalSupply
    {
        public Guid Id { get; set; }

        public DateTime DateOfHire { get; set; }

        public int Status { get; set; }

        public int Amount { get; set; }

        public Guid ItemId { get; set; }

        public Guid PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public virtual Item Item { get; set; }
    }
}