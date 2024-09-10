﻿namespace DTKH2024.SbinSolution.DataImporting.Excel;

public class ImportFromExcelDto
{
    /// <summary>
    /// Can be set when reading data from excel or when importing user
    /// </summary>
    public string Exception { get; set; }
        
    public bool CanBeImported()
    {
        return string.IsNullOrEmpty(Exception);
    }
}