using System.Collections.Generic;
using Abp;
using DTKH2024.SbinSolution.Chat.Dto;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
