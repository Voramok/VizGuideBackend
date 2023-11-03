using VizGuideBackend.Data;
using VizGuideBackend.Models;
using VizGuideBackend.Repository.ModelsIRepository;

namespace VizGuideBackend.Repository.ModelsRepository
{
    public class MemberProceduresRepository : GenericRepository<MemberProcedure>, IMemberProceduresRepository
    { 
		public MemberProceduresRepository(AppDbContext context) : base(context)
		{

		}

		public IEnumerable<MemberProcedure> GetMemberProceduresById(int Id)
		{
			return _context.MemberProcedures.Where(x => x.Id == Id);
		}
	}
}
