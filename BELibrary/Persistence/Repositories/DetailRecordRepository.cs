using System;
using System.Collections.Generic;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class DetailRecordRepository : Repository<DetailRecord>, IDetailRecordRepository
    {
        public DetailRecordRepository(PatientManagementDbContext context)
            : base(context)
        {
        }

        public PatientManagementDbContext PatientManagementDbContext
        {
            get { return Context as PatientManagementDbContext; }
        }
    }
}