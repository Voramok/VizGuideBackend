using VizGuideBackend.Data;
using VizGuideBackend.Models;
using VizGuideBackend.Repository.ModelsIRepository;

namespace VizGuideBackend.Repository.ModelsRepository
{
    public class PropertiesRepository : GenericRepository<Propertie>, IPropertiesRepository
    { 
		public PropertiesRepository(AppDbContext context) : base(context)
		{

		}

		public IEnumerable<Propertie> GetPropertiesById(int Id)
		{
			return _context.Properties.Where(x => x.Id == Id);
		}
	}
}
