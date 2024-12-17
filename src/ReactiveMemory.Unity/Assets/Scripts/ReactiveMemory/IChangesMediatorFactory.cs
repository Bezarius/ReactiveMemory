namespace ReactiveMemory
{
    public interface IChangesMediatorFactory
    {
        IChangesMediator<TElement> Create<TElement>();
    }
}