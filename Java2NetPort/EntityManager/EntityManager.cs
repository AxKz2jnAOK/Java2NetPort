using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI;

namespace Java2NetPort
{
    public class EntityManager : IEntytiManager
    {
        internal DbContext _DbContext { get; set; }
        public EntityManager(DbContext dbContext)
        {
            _DbContext = dbContext;
        }

        #region IEntytiManager Members
        public void Clear()
        {
            foreach (DbEntityEntry dbEntityEntry in _DbContext.ChangeTracker.Entries())
            {
                dbEntityEntry.State = EntityState.Detached;
            }
        }

        public void Flush()
        {
            _DbContext.SaveChanges();
        }

        public bool Contains<TEntity>(TEntity entity) where TEntity : class
        {
            return (_DbContext.Entry<TEntity>(entity).State == EntityState.Added
                    || _DbContext.Entry<TEntity>(entity).State == EntityState.Deleted
                    || _DbContext.Entry<TEntity>(entity).State == EntityState.Modified
                    || _DbContext.Entry<TEntity>(entity).State == EntityState.Unchanged);
        }

        public void Detach<TEntity>(TEntity entity)
        {
            ((IObjectContextAdapter)_DbContext).ObjectContext.Detach(entity);
        }
        public TEntity Find<TEntity>(object primaryKey) where TEntity:class
        {   
            return _DbContext.Set<TEntity>().Find(primaryKey);
        }
        public TEntity GetReference<TEntity>(object primaryKey) where TEntity : class
        {
            //Cast Dbcontext to get additional methods
            ObjectContext objectContext = ((IObjectContextAdapter)_DbContext).ObjectContext;
            ObjectSet<TEntity> set = objectContext.CreateObjectSet<TEntity>();
            //get primary key property
            string keyName = set.EntitySet.ElementType
                            .KeyMembers
                            .Select(k => k.Name)
                            .First();
            //Create entity
            TEntity entity = _DbContext.Set<TEntity>().Create();
            //Set primary key
            typeof(TEntity).GetProperty(keyName).SetValue(entity, primaryKey);            
            //_DbContext.Set<TEntity>().Attach(entity);
            //objectContext.Refresh(RefreshMode.StoreWins, entity);
            return entity;            
        }

        public void Persist<TEntity>(TEntity entity) where TEntity : class
        {
            _DbContext.Set<TEntity>().Add(entity);
        }

        public void Refresh<TEntity>(TEntity entity) where TEntity : class
        {
            //ObjectContext objectContext = ((IObjectContextAdapter)_DbContext).ObjectContext;
            //objectContext.Refresh(RefreshMode.StoreWins, entity);
            _DbContext.Entry<TEntity>(entity).Reload();
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            _DbContext.Set<TEntity>().Remove(entity);
        }

        #endregion

        public ICriteriaBuilder GetCriteriaBuilder()
        {
            return new CriteriaBuilder();
        }
    }
}
