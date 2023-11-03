using VizGuideBackend.Repository.ModelsIRepository;
using NLog;
using System.Data.SqlClient;

namespace VizGuideBackend.Data
{

    public class UnitOfWork : IUnitOfWork
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        private readonly AppDbContext _context;
        public IScriptsRepository Scripts { get; }
        public IBaseTypesRepository BaseTypes { get; }
        public IMemberProceduresRepository MemberProcedures { get; }
        public IPropertiesRepository Properties { get; }

        public UnitOfWork(AppDbContext appDbContext,
            IScriptsRepository scriptsRepository, 
            IBaseTypesRepository baseTypesRepository,
            IMemberProceduresRepository memberProceduresRepository,
            IPropertiesRepository propertiesRepository)
        {
            _context = appDbContext;
            Scripts = scriptsRepository;
            BaseTypes = baseTypesRepository;
            MemberProcedures = memberProceduresRepository;
            Properties = propertiesRepository;
        }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "Save to db error");
                throw;
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
