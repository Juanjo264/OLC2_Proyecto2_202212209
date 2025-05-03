using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

public static class ASTExporter
{
    public static string ConvertToDOT(IParseTree tree, Parser parser)
    {
        if (tree == null) return "digraph G { }";

        int nodeId = 0;
        var dot = "digraph G {\n";
        dot += "node [shape=box];\n";  
        dot += WalkTree(tree, parser, ref nodeId, out _);
        dot += "}";

        return dot;
    }

    private static string WalkTree(IParseTree tree, Parser parser, ref int nodeId, out string currentNodeName)
    {
        currentNodeName = $"node{nodeId++}";

        // Obtener etiqueta: si es un nodo de regla, obtener su nombre
        string label = tree is ParserRuleContext ctx
            ? parser.RuleNames[ctx.RuleIndex]
            : tree.GetText();

        label = EscapeLabelForDot(label);

        string result = $"{currentNodeName} [label=\"{label}\"];\n";

        for (int i = 0; i < tree.ChildCount; i++)
        {
            string childStr = WalkTree(tree.GetChild(i), parser, ref nodeId, out string childNodeName);
            result += childStr;
            result += $"{currentNodeName} -> {childNodeName};\n";
        }

        return result;
    }

    // Escapa correctamente para Graphviz
    private static string EscapeLabelForDot(string input)
    {
        return input
            .Replace("\\", "\\\\")  // escapamos backslashes primero
            .Replace("\"", "\\\"")  // luego escapamos comillas dobles
            .Replace("\n", "\\n")   // opcional: escapamos saltos de línea
            .Replace("\r", "");     // removemos retorno de carro si aparece
    }
}
