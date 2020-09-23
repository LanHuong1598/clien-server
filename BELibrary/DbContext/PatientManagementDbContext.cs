namespace BELibrary.DbContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BELibrary.Entity;

    public partial class PatientManagementDbContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<DetailPrescription> DetailPrescriptions { get; set; }

        public virtual DbSet<DetailRecord> DetailRecords { get; set; }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<MedicalSupply> MedicalSupplies { get; set; }

        public virtual DbSet<Medicine> Medicines { get; set; }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<Prescription> Prescriptions { get; set; }

        public virtual DbSet<Record> Records { get; set; }

        public virtual DbSet<Attachment> Attachments { get; set; }

        public virtual DbSet<AttachmentAssign> AttachmentAssigns { get; set; }

        public virtual DbSet<Faculty> Faculties { get; set; }

        public virtual DbSet<Doctor> Doctors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Medicine>()
                .HasMany(e => e.DetailPrescriptions)
                .WithRequired(e => e.Medicine)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.IndentificationCardId)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .HasMany(e => e.MedicalSupplies)
                .WithRequired(e => e.Patient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Record>()
                .HasMany(e => e.DetailRecords)
                .WithRequired(e => e.Record)
                .WillCascadeOnDelete(false);
        }
    }
}