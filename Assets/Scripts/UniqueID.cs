using System;

namespace ID
{
    public class UniqueID
    {
        public readonly string Value;
        public int ValueInt { get; }

        public UniqueID()
        {
            Value = Generate();
        }
        
        public UniqueID(string id)
        {
            Value = id;
            ValueInt = Math.Abs(Value.GetHashCode());
        }

        public override string ToString()
        {
            return Value;
        }
        public static string Generate()
        {
            byte[] gb = System.Guid.NewGuid().ToByteArray();
            return BitConverter.ToString(gb,0);
        }
        
        public static UniqueID Random => new UniqueID();

        public static implicit operator string(UniqueID id) => id.ToString();
        public static implicit operator int(UniqueID id) => id.ValueInt;

        public static bool operator ==(UniqueID id1, UniqueID id2)
        {
            if (id1 is null && id2 is null)
                return true;
            
            return id1 is {} && id2 is {} && id1.Equals(id2);
        }

        public static bool operator !=(UniqueID id1, UniqueID id2) => !(id1 == id2);
        
        public override bool Equals(object obj) => obj is UniqueID id && this.ValueInt == id.ValueInt;
        public bool Equals(UniqueID other) => ValueInt == other.ValueInt;
        
        public override int GetHashCode()
        {
            unchecked { return (ValueInt * 397) ^ (Value.GetHashCode()); }
        }
    }
}