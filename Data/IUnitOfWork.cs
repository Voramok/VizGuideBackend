using VizGuideBackend.Repository.ModelsIRepository;

namespace VizGuideBackend.Data
{

    public interface IUnitOfWork : IDisposable
    {
        IScriptsRepository Scripts { get; }
        IBaseTypesRepository BaseTypes { get; }
        IMemberProceduresRepository MemberProcedures { get; }
        IPropertiesRepository Properties { get; }
        int Complete();
    }
}
