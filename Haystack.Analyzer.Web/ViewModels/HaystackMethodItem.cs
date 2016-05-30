using Haystack.Analysis.ObjectModel;
using Haystack.Diagnostics.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Analyzer.Web.ViewModels
{
    public class HaystackMethodItem
    {
        public HaystackMethodItem()
        {
        }

        public HaystackMethodItem(IHaystackMethod method)
        {
            ClassName = method.ClassName;
            MethodName = method.MethodName;
            MethodParameters = string.Join(", ", method.MethodParameters.Select(ToString));
        }
        
        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public string MethodParameters { get; set; }

        private static string ToString(IHaystackMethodParameter parameter)
        {
            string modifier = string.Empty;
            switch (parameter.Modifier)
            {
                case ParameterModifier.Out:
                    modifier = "out ";
                    break;
                case ParameterModifier.Ref:
                    modifier = "ref ";
                    break; 
            }

            return modifier + parameter.ParameterType;
        }
    }
}
