using System;
using System.IO;

namespace UnambitiousFx.Core.CodeGen.Results.Extensions.ValueAccess
{
    /// <summary>
    /// Handles directory setup for value access extension generation.
    /// </summary>
    internal static class DirectorySetup
    {
        public static (string mainOutput, string tasksOutput, string valueTasksOutput) Prepare(string outputPath, string directoryName)
        {
            var mainOutput = Path.Combine(outputPath, directoryName);
            if (!Directory.Exists(mainOutput))
                Directory.CreateDirectory(mainOutput);

            var tasksOutput = Path.Combine(mainOutput, "Tasks");
            var valueTasksOutput = Path.Combine(mainOutput, "ValueTasks");
            if (!Directory.Exists(tasksOutput))
                Directory.CreateDirectory(tasksOutput);
            if (!Directory.Exists(valueTasksOutput))
                Directory.CreateDirectory(valueTasksOutput);
            return (mainOutput, tasksOutput, valueTasksOutput);
        }
    }
}
