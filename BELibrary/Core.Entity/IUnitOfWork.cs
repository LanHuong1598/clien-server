using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Core.Entity.Repositories;

namespace BELibrary.Core.Entity
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        ICategoryRepository Categories { get; }
        IDetailPrescriptionRepository DetailPrescriptions { get; }
        IItemRepository Items { get; }
        IDetailRecordRepository DetailRecords { get; }
        IMedicineRepository Medicines { get; }
        IPatientRepository Patients { get; }
        IMedicalSupplyRepository MedicalSupplies { get; }
        IPrescriptionRepository Prescriptions { get; }
        IRecordRepository Records { get; }
        IAttachmentAssignRepository AttachmentAssigns { get; }
        IAttachmentRepository Attachments { get; }
        IDoctorRepository Doctors { get; }
        IFacultyRepository Faculties { get; }

        int Complete();
    }
}