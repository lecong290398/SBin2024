﻿using System.Collections.Generic;
using System.Linq;

namespace DTKH2024.SbinSolution.Exporting;

public static class EntityExportHelper
{
    public static List<string> GetEntityColumnNames<TEntity>()
    {
        var properties = typeof(TEntity).GetProperties();
        
        var columns = properties.Select(p => p.Name).ToList();
            
        return columns;
    }
}