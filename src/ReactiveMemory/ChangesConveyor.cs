using System.Collections.Generic;

namespace ReactiveMemory
{
    public class ChangesConveyor
    {
        private readonly SparseSet<IDbChangesPublisher> _dbChangesPublishers = new SparseSet<IDbChangesPublisher>();
        private readonly Queue<IDbChangesPublisher> _publishersQueue = new Queue<IDbChangesPublisher>();
        private readonly IChangesMediatorFactory _changesMediatorFactory;

        public ChangesConveyor(IChangesMediatorFactory changesMediatorFactory)
        {
            _changesMediatorFactory = changesMediatorFactory;
        }

        public IChangesQueue<TElement> GetQueue<TElement>()
        {
            if(_changesMediatorFactory == null)
                return null;

            var typeId = TypeId<TElement>.Id;
            if (!_dbChangesPublishers.Contains(typeId))
            {
                _dbChangesPublishers.Add(typeId, new ChangesQueue<TElement>(this, _changesMediatorFactory.Create<TElement>()));
            }
            var publisher = _dbChangesPublishers[typeId];
            return publisher as IChangesQueue<TElement>;
        }
        
        internal void Enqueue(IDbChangesPublisher publisher)
        {
            _publishersQueue.Enqueue(publisher);
        }

        public void Publish()
        {
            while (_publishersQueue.Count > 0)
            {
                var publisher = _publishersQueue.Dequeue();
                publisher.PublishNext();
            }
        }

        public void Clear()
        {
            foreach (var publisher in _dbChangesPublishers)
            {
                publisher.Clear();
            }
        }
    }
}