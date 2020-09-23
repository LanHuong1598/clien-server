using System;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class MedicalSupplyRepository : Repository<MedicalSupply>, IMedicalSupplyRepository
    {
        public MedicalSupplyRepository(PatientManagementDbContext context)
            : base(context)
        {
        }

        public PatientManagementDbContext PatientManagementDbContext
        {
            get { return Context as PatientManagementDbContext; }
        }
    }
}