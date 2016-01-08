using System;
using System.Collections.Generic;
using System.IO;

namespace Haystack.Core
{
    public sealed class RelativePathResolver
    {
        private readonly string baseDirectory;

        public RelativePathResolver(string baseDirectory)
        {
            this.baseDirectory = baseDirectory;
        }

        public void ResolveIfNecessary(string path, Action<string> updateAction)
        {
            if (!string.IsNullOrWhiteSpace(path) && !Path.IsPathRooted(path))
            {
                updateAction(Path.GetFullPath(Path.Combine(baseDirectory, path)));
            }
        }

        public void ResolveIfNecessary(IList<string> paths)
        {
            if (paths != null)
            {
                for (int index = 0; index < paths.Count; index++)
                {
                    ResolveIfNecessary(paths[index], UpdateList(paths, index));
                }
            }
        }

        private static Action<string> UpdateList(IList<string> paths, int index)
        {
            return path =>
            {
                paths.RemoveAt(index);
                paths.Insert(index, path);
            };
        }
    }
}
