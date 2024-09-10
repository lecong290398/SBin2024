using System.Collections.Generic;
using System.IO;
using Abp.AspNetZeroCore.Net;
using Abp.Dependency;
using MiniExcelLibs;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel
{
    public abstract class MiniExcelExcelExporterBase : SbinSolutionServiceBase, ITransientDependency
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        
        protected MiniExcelExcelExporterBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        protected FileDto CreateExcelPackage(string fileName, List<Dictionary<string, object>> items)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            
            Save(items, file);

            return file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="file"></param>
        protected virtual void Save(List<Dictionary<string, object>> items, FileDto file)
        {
            using (var stream = new MemoryStream())
            {
                stream.SaveAs(items);
                _tempFileCacheManager.SetFile(file.FileToken, stream.ToArray());
            }
        }
    }
}
