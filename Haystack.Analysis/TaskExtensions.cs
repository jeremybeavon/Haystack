﻿using System;
using System.Threading.Tasks;

namespace Haystack.Analysis
{
    public static class TaskExtensions
    {
        public static void Then<TResult>(this Task<TResult> task, Action<TResult> action)
        {
            task.Wait();
            action(task.Result);
        }
    }
}
