using System;

namespace ReactiveMemory
{
    public interface IChangesMediator<TElement> : IObserver<EntityChange<TElement>>, IObservable<EntityChange<TElement>>
    {

    }
}