using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analysis.ObjectModel
{
    public sealed class HaystackMethodsWithRefactoring
    {
        [MessagePackMember(0)]
        public List<RefactoredMethod> RefactoredMethods { get; set; }

        [MessagePackMember(1)]
        public List<int> NonRefactoredMethodIndexes { get; set; }

        public List<HaystackMethod> NonRefactoredMethods { get; set; }
    }
}
