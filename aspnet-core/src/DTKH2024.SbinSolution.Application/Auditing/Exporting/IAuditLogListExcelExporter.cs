using System.Collections.Generic;
using DTKH2024.SbinSolution.Auditing.Dto;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
