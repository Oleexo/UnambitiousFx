using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal interface IMethodWriter {
    IEnumerable<string> Usings { get; }
    void Write(IndentedTextWriter writer);
}
