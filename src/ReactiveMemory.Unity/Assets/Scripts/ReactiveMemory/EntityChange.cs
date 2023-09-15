using System.Collections.Generic;
using System;

namespace ReactiveMemory
{
    public readonly struct EntityChange<TEntity>
    {
        public EEntityChangeType Change { get; }
        public TEntity Entity { get; }
        public TEntity Old { get; }

        public EntityChange(EEntityChangeType change, TEntity entity)
        {
            Change = change;
            Entity = entity;
            Old = default;
        }

        public EntityChange(EEntityChangeType change, TEntity entity, TEntity old)
        {
            Change = change;
            Entity = entity;
            Old = old;
        }

        public bool IsChanged<TProp>(Func<TEntity, TProp> selector)
        {
            return Change != EEntityChangeType.Update ||
                Comparer<TProp>.Default.Compare(selector(Entity), selector(Old)) != 0;
        }
    }
}