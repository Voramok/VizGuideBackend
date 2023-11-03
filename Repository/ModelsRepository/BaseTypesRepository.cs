using VizGuideBackend.Data;
using VizGuideBackend.Models;
using VizGuideBackend.Repository.ModelsIRepository;

namespace VizGuideBackend.Repository.ModelsRepository
{
    public class BaseTypesRepository : GenericRepository<BaseType>, IBaseTypesRepository
    { 
		public BaseTypesRepository(AppDbContext context) : base(context)
		{

		}

        public IEnumerable<BaseType> GetBaseTypesById(int Id)
		{
			return _context.BaseTypes.Where(x => x.Id == Id);
		}
    }
}
