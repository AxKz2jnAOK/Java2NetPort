using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java2NetPort.CriteriaAPI;

namespace Java2NetPort
{
    public interface IEntytiManager
    {
        /// <summary>
        /// Clear the persistence context, causing all managed entities to become detached. Changes made to entities that have not been flushed to the database will not be persisted.
        /// </summary>
        void Clear();
        /// <summary>
        /// Synchronize the persistence context to the underlying database.
        /// </summary>
        void Flush();
        /// <summary>
        /// Check if the instance is a managed entity instance belonging to the current persistence context.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns>boolean indicating if entity is in persistence context</returns>
        ///<exception cref="InvalidOperationException - if not an entity"
        bool Contains<TEntity>(TEntity entity) where TEntity : class;
        void Detach<TEntity>(TEntity entity);
        TEntity Find<TEntity>(object primaryKey) where TEntity : class;
        TEntity GetReference<TEntity>(object primaryKey) where TEntity : class;
        /// <summary>
        /// Make an instance managed and persistent.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>        
        void Persist<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// Refresh the state of the instance from the database, overwriting changes made to the entity, if any.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Refresh<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// Remove the entity instance.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void Remove<TEntity>(TEntity entity) where TEntity : class;


        ICriteriaBuilder GetCriteriaBuilder();

    }
}
