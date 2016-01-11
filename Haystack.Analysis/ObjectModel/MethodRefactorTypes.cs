using System;

namespace Haystack.Analysis.ObjectModel
{
    [Flags]
    public enum MethodRefactorTypes
    {
        None,
        MethodRename = 0x1,
        TypeRename = 0x2,
        ParameterAdded = 0x4,
        ParameterRemoved = 0x8,
        ParameterChanged = 0x10,
        ReturnTypeChanged = 0x20
    }
}
