using MsgPack.Serialization;

namespace Haystack.Diagnostics.ObjectModel
{
    public sealed class MethodParameter
    {
        [MessagePackMember(0)]
        public int ParameterTypeIndex { get; set; }

        public ObjectType ParameterType { get; set; }

        [MessagePackMember(1)]
        public string ParameterName { get; set; }

        [MessagePackMember(2)]
        public ParameterModifier Modifier { get; set; }

        [MessagePackMember(3)]
        public Value Value { get; set; }

        [MessagePackMember(4)]
        public Value OutputValue { get; set; }
    }
}
