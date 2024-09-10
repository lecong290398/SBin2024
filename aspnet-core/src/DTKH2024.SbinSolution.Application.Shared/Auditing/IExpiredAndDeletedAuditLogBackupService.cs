using System.Collections.Generic;
using Abp.Auditing;

namespace DTKH2024.SbinSolution.Auditing
{
    public interface IExpiredAndDeletedAuditLogBackupService
    {
        bool CanBackup();
        
        void Backup(List<AuditLog> auditLogs);
    }
}