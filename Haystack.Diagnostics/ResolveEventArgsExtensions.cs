using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Haystack.Diagnostics
{
    public static class ResolveEventArgsExtensions
    {
        public static string AssemblyName(this ResolveEventArgs args)
        {
            return new AssemblyName(args.Name).Name;
        }
    }
}
