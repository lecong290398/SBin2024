using DTKH2024.SbinSolution.TransactionBins.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.TransactionBins
{
    public class CreateOrEditTransactionBinModalViewModel
    {
        public CreateOrEditTransactionBinDto TransactionBin { get; set; }

        public string DeviceName { get; set; }

        public string UserName { get; set; }

        public string TransactionStatusName { get; set; }

        public List<TransactionBinTransactionStatusLookupTableDto> TransactionBinTransactionStatusList { get; set; }

        public bool IsEditMode => TransactionBin.Id.HasValue;
    }
}