using System.Collections.Generic;
using Abp.Dependency;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}