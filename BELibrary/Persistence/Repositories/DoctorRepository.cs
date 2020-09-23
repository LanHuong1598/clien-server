using System;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(PatientManagementDbContext context)
            : base(context)
        {
        }

        public PatientManagementDbContext PatientManagementDbContext => Context;
    }
}