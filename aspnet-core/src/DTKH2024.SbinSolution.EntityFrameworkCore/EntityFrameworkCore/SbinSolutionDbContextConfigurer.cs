using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace DTKH2024.SbinSolution.EntityFrameworkCore
{
    public static class SbinSolutionDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SbinSolutionDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SbinSolutionDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}