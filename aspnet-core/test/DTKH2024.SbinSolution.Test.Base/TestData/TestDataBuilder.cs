using DTKH2024.SbinSolution.EntityFrameworkCore;

namespace DTKH2024.SbinSolution.Test.Base.TestData
{
    public class TestDataBuilder
    {
        private readonly SbinSolutionDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(SbinSolutionDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            new TestOrganizationUnitsBuilder(_context, _tenantId).Create();
            new TestSubscriptionPaymentBuilder(_context, _tenantId).Create();
            new TestEditionsBuilder(_context).Create();

            _context.SaveChanges();
        }
    }
}
