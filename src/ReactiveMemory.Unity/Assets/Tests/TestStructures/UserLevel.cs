using MessagePack;

namespace ReactiveMemory.Tests
{
    [MemoryTable("UserLevel"), MessagePackObject(true)]
    public record UserLevel
    {
        [PrimaryKey]
        public int Level { get; set; }
        [SecondaryKey(0)]
        public int Exp { get; set; }

        public UserLevel(int Level, int Exp)
        {
            this.Level = Level;
            this.Exp = Exp;
        }
    }
}