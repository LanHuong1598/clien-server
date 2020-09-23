using System;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        public AttachmentRepository(PatientManagementDbContext context)
            : base(context)
        {
        }

        public PatientManagementDbContext PatientManagementDbContext => Context;
    }
}