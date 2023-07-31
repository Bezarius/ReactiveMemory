using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveMemory.Tests.TestStructures
{

    [MemoryTable("people"), MessagePackObject(true)]
    public class PersonModel
    {
        [SecondaryKey(0), NonUnique]
        [SecondaryKey(1, keyOrder: 1), NonUnique]
        public string LastName { get; set; }

        [SecondaryKey(2), NonUnique]
        [SecondaryKey(1, keyOrder: 0), NonUnique]
        public string FirstName { get; set; }

        [PrimaryKey] public string RandomId { get; set; }

        public PersonModel(string LastName, string FirstName, string RandomId)
        {
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.RandomId = RandomId;
        }
    }
}
