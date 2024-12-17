using System;

namespace ReactiveMemory
{
    public interface IChangesQueue<TElement>
    {
        IObservable<EntityChange<TElement>> OnChange { get; }
        void EnqueueAdd(TElement added);
        void EnqueueRemove(TElement element);
        void EnqueueUpdate(TElement updated, TElement old);
    }
}