using MsgPack.Serialization;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class ObjectInstance : IObjectInstance
    {
        [MessagePackMember(0)]
        public int TypeIndex { get; set; }

        public ObjectType Type { get; set; }

        IObjectType IObjectInstance.Type
        {
            get { return Type; }
        }
    }
}
