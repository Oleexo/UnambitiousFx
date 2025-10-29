using System;

namespace UnambitiousFx.Core.CodeGen.Results.Extensions.ValueAccess
{
    /// <summary>
    /// Contains methods for generating partial extension files for value access.
    /// </summary>
    internal static class PartialGenerators
    {
        public static void GenerateAllPartials(ResultValueAccessExtensionsCodeGenerator generator, ushort numberOfArity, string mainOutput, string tasksOutput, string valueTasksOutput)
        {
            generator.GenerateToNullablePartialFiles(numberOfArity, mainOutput);
            generator.GenerateValueOrPartialFiles(numberOfArity, mainOutput);
            generator.GenerateValueOrThrowPartialFiles(numberOfArity, mainOutput);
            generator.GenerateValueOrAsyncPartialFiles(numberOfArity, tasksOutput, valueTasksOutput);
            generator.GenerateValueOrThrowAsyncPartialFiles(numberOfArity, tasksOutput, valueTasksOutput);
        }
    }
}
