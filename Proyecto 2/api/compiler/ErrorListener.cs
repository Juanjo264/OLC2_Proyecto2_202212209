using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Collections.Generic;
using System.IO;

public class SemanticError : Exception
{
    private string message;
    private Antlr4.Runtime.IToken token;

    public SemanticError(string message, Antlr4.Runtime.IToken token)
    {
        this.message = message;
        this.token = token;
    }

    public override string Message
    {
        get
        {
            return message + " en linea " + token.Line + ", columna " + token.Column;
        }
    }
}

public class Error
{
    public string Type { get; set; }
    public string Message { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }

    public Error(string type, string message, int line, int column)
    {
        Type = type;
        Message = message;
        Line = line;
        Column = column;
    }
}

public class LexicalErrorListener : BaseErrorListener, IAntlrErrorListener<int>
{
    public List<Error> Errors { get; } = new List<Error>();

    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        Errors.Add(new Error("Lexical", msg, line, charPositionInLine));
        Console.WriteLine("Error lexico en linea " + line + ", columna " + charPositionInLine + ": " + msg);
    }
}

public class SyntaxErrorListener : BaseErrorListener
{
    public List<Error> Errors { get; } = new List<Error>();

    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
    {
        Errors.Add(new Error("Syntax", msg, line, charPositionInLine));
        Console.WriteLine("Error sintactico en linea " + line + ", columna " + charPositionInLine + ": " + msg);
    }
}