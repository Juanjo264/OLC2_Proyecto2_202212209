
using System.Reflection.Metadata;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

public class Symbol
{
    public string ID { get; set; }
    public string TypeSymbol { get; set; }
    public string DataType { get; set; }
    public string Scope { get; set; }
    public int Line { get; set; }
    public int Column { get; set; }

    public Symbol(string id, string typeSymbol, string dataType, string scope, int line, int column)
    {
        ID = id;
        TypeSymbol = typeSymbol;
        DataType = dataType;
        Scope = scope;
        Line = line;
        Column = column;
    }
}

public class SymbolTable
{
    public List<Symbol> Symbols { get; set; } = new List<Symbol>();

    public void AddSymbol(string id, string typeSymbol, string dataType, string scope, int line, int column)
    {
        Symbols.Add(new Symbol(id, typeSymbol, dataType, scope, line, column));
    }
}

public class CompilerVisitor : LanguageBaseVisitor<Object?>
{

    public ArmGenerator c = new ArmGenerator();
    public CompilerVisitor()
    {
    }

    public override Object? VisitProgram(LanguageParser.ProgramContext context)
    {
        foreach (var listainstrucciones in context.listainstrucciones())
        {
            Visit(listainstrucciones);
        }
        return null;
    }

    public override Object? VisitExprecionInstruccion(LanguageParser.ExprecionInstruccionContext context)
    {
        return null;
    }

    public override Object? VisitPrint(LanguageParser.PrintContext context)
    {
        c.Comment("Print statement");
        Visit(context.impresiones());  
        return null;
    }

    public override Object? VisitImpresiones(LanguageParser.ImpresionesContext context)
    {
        if (context.impresiones() != null)
        {
            Visit(context.impresiones()); 
        }
        c.Comment("visiting expression");
        Visit(context.expr()); 
        c.Comment("Popping value to print");
        c.Pop(Register.X0);   
        c.PrintInteger(Register.X0); 
        return null;
    }

    public override Object? VisitIdexpresion(LanguageParser.IdexpresionContext context)
    {
        return null;
    }

    public override Object? VisitParens(LanguageParser.ParensContext context)
    {
        return null;
    }

    public override Object? VisitNegate(LanguageParser.NegateContext context)
    {
        return null;
    }

    public override Object? VisitInt(LanguageParser.IntContext context)
    {
        var value = context.INT().GetText();
        c.Comment($"Constant: {value}");
        c.Mov(Register.X0 , int.Parse(value));
        c.Push(Register.X0);
        return null;
    }

    public override Object? VisitMulDiv(LanguageParser.MulDivContext context)
    {
        return null;
    }

    public override Object? VisitAddSub(LanguageParser.AddSubContext context)
    {
        c.Comment("Add/Subtract operation");
        var operation = context.op.Text;

        c.Comment($"Operation left: {operation}");
        Visit(context.expr(0));
        c.Comment($"Operation right: {operation}");
        Visit(context.expr(1));

        c.Pop(Register.X1); // Pop the second operand into X1
        c.Pop(Register.X0); // Pop the first operand into X0

        if (operation == "+")
        {
            c.Add(Register.X0, Register.X0, Register.X1); // X0 = X0 + X1
        }
        else if (operation == "-")
        {
            c.Sub(Register.X0, Register.X0, Register.X1); // X0 = X0 - X1
        }
        c.Comment($"pushing result");
        c.Push(Register.X0); // Push the result back onto the stack
        return null;
    }

    public override Object? VisitMod(LanguageParser.ModContext context)
    {
        return null;
    }

    public override Object? VisitIncremento(LanguageParser.IncrementoContext context)
    {
        return null;
    }

    public override Object? VisitDeclaracionVar(LanguageParser.DeclaracionVarContext context)
    {
        return null;
    }

    public override Object? VisitDeclaracionImplicita(LanguageParser.DeclaracionImplicitaContext context)
    {
        return null;
    }

    public override Object? VisitAsignarVar(LanguageParser.AsignarVarContext context)
    {
        return null;
    }

    public override Object? VisitAsignarSliceCompleto(LanguageParser.AsignarSliceCompletoContext context)
    {
        return null;
    }

    public override Object? VisitFloat(LanguageParser.FloatContext context)
    {
        return null;
    }

    public override Object? VisitBoleanTrueExpresion(LanguageParser.BoleanTrueExpresionContext context)
    {
        return null;
    }

    public override Object? VisitBoleanFalseExpresion(LanguageParser.BoleanFalseExpresionContext context)
    {
        return null;
    }

    public override Object? VisitCadenaExpresion(LanguageParser.CadenaExpresionContext context)
    {
        return null;
    }

    public override Object? VisitCaracterExpresion(LanguageParser.CaracterExpresionContext context)
    {
        return null;
    }

    public override Object? VisitOperadorNegacion(LanguageParser.OperadorNegacionContext context)
    {
        return null;
    }

    public override Object? VisitRelational(LanguageParser.RelationalContext context)
    {
        return null;
    }

    public override Object? VisitEqualitys(LanguageParser.EqualitysContext context)
    {
        return null;
    }

    public override Object? VisitIncrementoDecremento(LanguageParser.IncrementoDecrementoContext context)
    {
        return null;
    }

    public override Object? VisitLogicos(LanguageParser.LogicosContext context)
    {
        return null;
    }

    public override Object? VisitBloqueInstrucciones(LanguageParser.BloqueInstruccionesContext context)
    {
        return null;
    }

    public override Object? VisitIfInstruccion(LanguageParser.IfInstruccionContext context)
    {
        return null;
    }

    public override Object? VisitWhileInstruccion(LanguageParser.WhileInstruccionContext context)
    {
        return null;
    }

    public override Object? VisitSwitchInstruccion(LanguageParser.SwitchInstruccionContext context)
    {
        return null;
    }

    public override Object? VisitDeclaracionSlice(LanguageParser.DeclaracionSliceContext context)
    {
        return null;
    }

    public override Object? VisitDeclaracionSlicevacio(LanguageParser.DeclaracionSlicevacioContext context)
    {
        return null;
    }

    public override Object? VisitNilExpresion(LanguageParser.NilExpresionContext context)
    {
        return null;
    }

    public override Object? VisitDeclaracionSlicemulti(LanguageParser.DeclaracionSlicemultiContext context)
    {
        return null;
    }

    public override Object? VisitAccesoSlice(LanguageParser.AccesoSliceContext context)
    {
        return null;
    }

    public override Object? VisitAsignarSlice(LanguageParser.AsignarSliceContext context)
    {
        return null;
    }

    public override Object? VisitAsignarSliceMulti(LanguageParser.AsignarSliceMultiContext context)
    {
        return null;
    }

    public override Object? VisitAccesoSliceMulti(LanguageParser.AccesoSliceMultiContext context)
    {
        return null;
    }

    public override Object? VisitForCondicion(LanguageParser.ForCondicionContext context)
    {
        return null;
    }

    public override Object? VisitForincicializacion(LanguageParser.ForincicializacionContext context)
    {
        return null;
    }

    public override Object? VisitForRange(LanguageParser.ForRangeContext context)
    {
        return null;
    }

    public override Object? VisitBreakInstruccion(LanguageParser.BreakInstruccionContext context)
    {
        return null;
    }

    public override Object? VisitReturnInstruccion(LanguageParser.ReturnInstruccionContext context)
    {
        return null;
    }

    public override Object? VisitContinueInstruccion(LanguageParser.ContinueInstruccionContext context)
    {
        return null;
    }

    public override Object? VisitCallee(LanguageParser.CalleeContext context)
    {
        return null;
    }

    public override Object? VisitFuncdlc(LanguageParser.FuncdlcContext context)
    {
        return null;
    }

    public override Object? VisitStructdcl(LanguageParser.StructdclContext context)
    {
        return null;
    }

    public override Object? VisitNew(LanguageParser.NewContext context)
    {
        return null;
    }

    public override Object? VisitModuleFuncCall(LanguageParser.ModuleFuncCallContext context)
    {
        return null;
    }

    public override Object? VisitInstanciaStruct(LanguageParser.InstanciaStructContext context)
    {
        return null;
    }
}

