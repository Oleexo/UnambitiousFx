using System.CodeDom.Compiler;

namespace UnambitiousFx.Core.CodeGen.Design;

internal interface ITypeDefinitionWriter {
    IEnumerable<string> Usings { get; }
    void Write(IndentedTextWriter writer);
}
