
using System.Reflection.Metadata;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
   
    // public class Symbol
    // {
    //     public string ID { get; set; }
    //     public string TypeSymbol { get; set; }
    //     public string DataType { get; set; }
    //     public string Scope { get; set; }
    //     public int Line { get; set; }
    //     public int Column { get; set; }

    //     public Symbol(string id, string typeSymbol, string dataType, string scope, int line, int column)
    //     {
    //         ID = id;
    //         TypeSymbol = typeSymbol;
    //         DataType = dataType;
    //         Scope = scope;
    //         Line = line;
    //         Column = column;
    //     }
    // }

    // public class SymbolTable
    // {
    //     public List<Symbol> Symbols { get; set; } = new List<Symbol>();

    //     public void AddSymbol(string id, string typeSymbol, string dataType, string scope, int line, int column)
    //     {
    //         Symbols.Add(new Symbol(id, typeSymbol, dataType, scope, line, column));
    //     }
    // }

    

public class InterpreterVisitor : LanguageBaseVisitor<ValueWrapper>{


public Environment currentEnvironment;

public InterpreterVisitor()
{
    currentEnvironment = new Environment(null);
    Embeded.Generate(currentEnvironment);
}


public ValueWrapper defaultValue = new VoidValue();
public string output = "";
    public SymbolTable symbolTable = new SymbolTable();
    public static SymbolTable GlobalSymbolTable = new SymbolTable();


//visitprogram
public override ValueWrapper VisitProgram(LanguageParser.ProgramContext context)
{
    // registrar primero todas las funciones 
    foreach (var declaracion in context.listainstrucciones())
    {
        if (declaracion.funcdlc() != null)
        {
            Visit(declaracion.funcdlc());
        }
    }

    //  registrar structs 
    foreach (var declaracion in context.listainstrucciones())
    {
        if (declaracion.structdcl() != null)
        {
            Visit(declaracion.structdcl());
        }
    }

    // Ejecutar variables, instrucciones 
    foreach (var declaracion in context.listainstrucciones())
    {
        if (declaracion.variables() != null || declaracion.instruccion() != null)
        {
            Visit(declaracion);
        }
    }

    return defaultValue;
}

    public override ValueWrapper VisitExprecionInstruccion( LanguageParser.ExprecionInstruccionContext context)
    {
        return Visit(context.expr());
    }

public override ValueWrapper VisitPrint(LanguageParser.PrintContext context) {
    if (context.impresiones() != null) {
        Visit(context.impresiones());
        output += "\n"; 
    }
    return defaultValue;
}
// Add this method to the InterpreterVisitor class
public Type ObtenerTipo(string typeName, object? context) {
    // Implement logic to map typeName to a Type object
    return Type.GetType(typeName) ?? throw new ArgumentException($"Type '{typeName}' not found.");
}

}

