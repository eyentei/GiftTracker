using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftTrackerClasses
{
    public class GTRepository<T> where T : class

    {
        private DbSet<T> dbSet;
        private GTContext context;

        public GTRepository(GTContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
            Save();
        }

        public void Update(T entity)
        {
            var e = dbSet.Find(entity);
            if (entity == null)
            {
                return;
            }

            context.Entry(entity).CurrentValues.SetValues(e);
        }

        public void AddRange(ICollection<T> entities)
        {
            dbSet.AddRange(entities);
            Save();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            Save();
        }

        public ICollection<T> GetAll()
        {
            dbSet.Load();
            return dbSet.Local;
        }

        public T GetById<U>(U id)
        {
            return dbSet.Find(id);
        }

        private void Save()
        {
            context.SaveChanges();
        }
    }
}
