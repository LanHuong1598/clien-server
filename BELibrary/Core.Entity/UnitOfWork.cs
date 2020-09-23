using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Persistence.Repositories;

namespace BELibrary.Core.Entity
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PatientManagementDbContext _context;

        public UnitOfWork(PatientManagementDbContext context)
        {
            _context = context;
            Accounts = new AccountRepository(_context);
            Categories = new CategoryRepository(_context);
            DetailPrescriptions = new DetailPrescriptionRepository(_context);
            Items = new ItemRepository(_context);
            DetailRecords = new DetailRecordRepository(_context);
            Medicines = new MedicineRepository(_context);
            Patients = new PatientRepository(_context);
            MedicalSupplies = new MedicalSupplyRepository(_context);
            Prescriptions = new PrescriptionRepository(_context);
            Records = new RecordRepository(_context);
            AttachmentAssigns = new AttachmentAssignRepository(_context);
            Attachments = new AttachmentRepository(_context);
            Doctors = new DoctorRepository(_context);
            Faculties = new FacultyRepository(_context);
        }

        public IAccountRepository Accounts { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IDetailPrescriptionRepository DetailPrescriptions { get; private set; }
        public IItemRepository Items { get; private set; }
        public IDetailRecordRepository DetailRecords { get; private set; }
        public IMedicineRepository Medicines { get; private set; }
        public IPatientRepository Patients { get; private set; }
        public IMedicalSupplyRepository MedicalSupplies { get; private set; }
        public IPrescriptionRepository Prescriptions { get; private set; }
        public IRecordRepository Records { get; private set; }
        public IAttachmentAssignRepository AttachmentAssigns { get; private set; }
        public IAttachmentRepository Attachments { get; private set; }
        public IFacultyRepository Faculties { get; private set; }
        public IDoctorRepository Doctors { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}