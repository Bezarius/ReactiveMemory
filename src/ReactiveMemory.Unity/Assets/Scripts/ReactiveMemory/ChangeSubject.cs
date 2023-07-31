using System;
//#if UNIRX
using UniRx;
//#endif

namespace ReactiveMemory
{
    public class UniRxSubjectFactory : IChangesMediatorFactory
    {
        public static UniRxSubjectFactory Default = new();

        public IChangesMediator<TElement> Create<TElement>()
        {
            return new UniRxSubject<TElement>();
        }
    }


    public class UniRxSubject<TElement> : IChangesMediator<TElement>
    {
        private readonly Subject<EntityChange<TElement>> _subject = new();

        public void OnCompleted()
        {
            _subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            _subject.OnError(error);
        }

        public void OnNext(EntityChange<TElement> value)
        {
            _subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<EntityChange<TElement>> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}
