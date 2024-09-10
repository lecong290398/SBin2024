using System.Collections.Generic;
using Abp.Dependency;

namespace DTKH2024.SbinSolution.DataImporting.Excel;

public interface IExcelDataReader<TEntityDto> : ITransientDependency
{
    List<TEntityDto> GetEntitiesFromExcel(byte[] fileBytes);
}