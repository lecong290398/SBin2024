using DTKH2024.SbinSolution.EntityFrameworkCore;

namespace DTKH2024.SbinSolution.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly SbinSolutionDbContext _context;

        public InitialHostDbBuilder(SbinSolutionDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
