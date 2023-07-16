namespace ReactiveMemory
{
    internal static class TypeIdGenerator
    {
        private static int _index;

        public static int Index => ++_index;
    }
}