namespace ReactiveMemory
{
    public interface IDbChangesPublisher
    {
        void PublishNext();
        void Clear();
    }
}