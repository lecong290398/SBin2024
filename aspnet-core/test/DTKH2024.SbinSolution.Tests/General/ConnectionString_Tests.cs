using System.Data.SqlClient;
using Shouldly;
using Xunit;

namespace DTKH2024.SbinSolution.Tests.General
{
    // ReSharper disable once InconsistentNaming
    public class ConnectionString_Tests
    {
        [Fact]
        public void SqlConnectionStringBuilder_Test()
        {
            var csb = new SqlConnectionStringBuilder("Server=localhost; Database=SbinSolution; Trusted_Connection=True;");
            csb["Database"].ShouldBe("SbinSolution");
        }
    }
}
