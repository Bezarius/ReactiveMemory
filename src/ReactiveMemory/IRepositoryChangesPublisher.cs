using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveMemory
{
    public interface IRepositoryChangesPublisher
    {
        void Publish();
        void Clear();
    }

    public interface IRepositoryChangesPublisher<TEntity> : IRepositoryChangesPublisher
    {
        void Add(TEntity entity);
        void Update(TEntity old, TEntity entity);
        void Remove(TEntity entity);
    }
}
