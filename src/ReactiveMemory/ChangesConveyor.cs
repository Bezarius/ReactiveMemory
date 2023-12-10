using System;
using System.Collections.Generic;

namespace ReactiveMemory
{
    public class ChangesConveyor : IDisposable
    {
        private readonly SparseSet<IDbChangesPublisher> _dbChangesPublishers = new SparseSet<IDbChangesPublisher>();
        private readonly Queue<IDbChangesPublisher> _publishersQueue = new Queue<IDbChangesPublisher>();
        private readonly Queue<Queue<Action>> _publishActionQueue = new Queue<Queue<Action>>();

        private readonly IChangesMediatorFactory _changesMediatorFactory;
        
        private bool _isPublishing;

        public ChangesConveyor(IChangesMediatorFactory changesMediatorFactory)
        {
            _changesMediatorFactory = changesMediatorFactory;
        }

        public IChangesQueue<TElement> GetQueue<TElement>()
        {
            if (_changesMediatorFactory == null)
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

        

        private Queue<Action> PrepareToPublish()
        {
            var q = new Queue<Action>();
            while (_publishersQueue.Count > 0)
            {
                var publisher = _publishersQueue.Dequeue();
                var publishAction = publisher.Prepare();
                q.Enqueue(publishAction);
            }
            return q;
        }

        // todo: right now ChangesConveyor know nothing about transaction and that problem for design of the ReactiveMemory
        // i should rework it to make Publish() method transactional with simple logic 
        public void Publish()
        {
            // IDbChangesPublisher shared for all transactions
            // so we should enqueue all publishers before publishing to preserve them
            // when we publish we can change db and this will lead to publishing of other modifications
            // but is some cases changes could be rolled back and we should clean queue, because it shared, changes could be cleared

            var preparation = PrepareToPublish();
            if (preparation.Count > 0)
                _publishActionQueue.Enqueue(preparation);

            // this flag is needed to prevent recursive calls
            // because PublishNext() can lead to changes in db
            // and this will lead to recursive call of Publish()
            // recursive call of Publish() could lead to order disorder
            if (_isPublishing) return;

            _isPublishing = true;

            while (_publishActionQueue.Count > 0)
            {
                var q = _publishActionQueue.Dequeue();
                while (q.Count > 0)
                {
                    var action = q.Dequeue();
                    action.Invoke();
                }
            }

            _isPublishing = false;
        }

        public void Clear()
        {
            foreach (var publisher in _dbChangesPublishers)
            {
                publisher.Clear();
            }
            _publishersQueue.Clear();
            _publishActionQueue.Clear();
        }

        public void Dispose()
        {
            foreach (var publisher in _dbChangesPublishers)
            {
                publisher.OnCompleted();
            }
        }
    }
}