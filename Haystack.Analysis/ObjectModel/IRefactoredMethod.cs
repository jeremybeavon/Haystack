namespace Haystack.Analysis.ObjectModel
{
    public interface IRefactoredMethod : IHaystackMethod
    {
        IHaystackMethod PassingMethod { get; }

        IHaystackMethod FailingMethod { get; }

        MethodRefactorTypes RefactorTypes { get; }
    }
}
