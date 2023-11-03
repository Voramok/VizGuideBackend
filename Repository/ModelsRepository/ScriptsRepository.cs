using VizGuideBackend.Data;
using VizGuideBackend.Models;
using VizGuideBackend.Repository.ModelsIRepository;

namespace VizGuideBackend.Repository.ModelsRepository
{
    public class ScriptsRepository : GenericRepository<Script>, IScriptsRepository
	{ 
		public ScriptsRepository(AppDbContext context) : base(context)
		{

		}

		public IEnumerable<Script> GetScriptsById(int Id)
		{
			return _context.Scripts.Where(x => x.Id == Id);
		}
	}
}
