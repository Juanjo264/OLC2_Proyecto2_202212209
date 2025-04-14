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
    dot += WalkTree(tree, parser, ref nodeId);
    dot += "}";

    return dot;
}

private static string WalkTree(IParseTree tree, Parser parser, ref int nodeId)
{
    string nodeName = $"node{nodeId++}";
    string label = tree is ParserRuleContext ctx ? parser.RuleNames[ctx.RuleIndex] : tree.GetText();

    label = label.Replace("\"", "\\\"");

    string result = $"{nodeName} [label=\"{label}\"];\n";

    for (int i = 0; i < tree.ChildCount; i++)
    {
        string childName = $"node{nodeId}";
        result += WalkTree(tree.GetChild(i), parser, ref nodeId);
        result += $"{nodeName} -> {childName};\n";
    }

    return result;
}
    
}
