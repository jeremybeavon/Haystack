using MsgPack.Serialization;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class ObjectInstance
    {
        [MessagePackMember(0)]
        public int TypeIndex { get; set; }

        public ObjectType Type { get; set; }
    }
}
