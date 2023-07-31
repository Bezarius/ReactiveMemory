namespace ReactiveMemory
{
    internal static class TypeId<T>
    {
        // ReSharper disable once StaticMemberInGenericType
        public static int Id { get; } = TypeIdGenerator.Index;
    }
}