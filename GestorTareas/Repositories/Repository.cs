using GestorTareas.Models.Entities;

namespace GestorTareas.Repositories
{
    public class Repository<T> where T : class
    {
        public Repository(RegistroTareasContext context)
        {
            Context = context;
        }

        public RegistroTareasContext Context { get; }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public T? Get(object id)
        {
            return Context.Find<T>(id);
        }

        public IQueryable<T> Query()
        {
            return Context.Set<T>().AsQueryable();
        }

        public void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public void Delete(object id)
        {
            T? entity = Get(id);

            if (entity != null)
            {
                Context.Remove(entity);
                Context.SaveChanges();
            }
        }
    }
}
