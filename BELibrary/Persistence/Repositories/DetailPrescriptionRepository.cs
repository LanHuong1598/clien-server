using System;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class DetailPrescriptionRepository : Repository<DetailPrescription>, IDetailPrescriptionRepository
    {
        public DetailPrescriptionRepository(PatientManagementDbContext context)
            : base(context)
        {
        }

        public PatientManagementDbContext PatientManagementDbContext
        {
            get { return Context as PatientManagementDbContext; }
        }
    }
}