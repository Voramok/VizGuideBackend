using Microsoft.EntityFrameworkCore;
using NLog;
using System.Data.SqlClient;
using VizGuideBackend.Data;

namespace VizGuideBackend.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        protected readonly AppDbContext _context;

        public DbSet<T> table;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return table.ToList();
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "GetAll from db error");
                throw;
            }

        }
        public T GetById(object id)
        {
            try
            {
                return table.Find(id);
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "GetById from db error");
                throw;
            }

        }

        public void Insert(T obj)
        {
            try
            {
                table.Add(obj);
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "Insert to db error");
                throw;
            }
        }

        public void Update(T obj)
        {
            try
            {
                //Update only modified rows
                table.Attach(obj);
                //Update all rows
                //_context.Entry(obj).State = EntityState.Modified;
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "Update in db error");
                throw;
            }
        }

        public void Delete(object id)
        {
            try
            {
                T existing = GetById(id);

                if (existing != null)
                {
                    table.Remove(existing);
                }
            }
            catch (SqlException ex)
            {
                logger.Error(ex, "Delete from db error");
                throw;
            }

        }
    }
}
