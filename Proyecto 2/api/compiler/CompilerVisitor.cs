
using System.Reflection.Metadata;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Globalization;

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
        c.Comment("Expression instruction");
        Visit(context.expr());
        c.Comment("Popping value to discard");
        c.PopObject(Register.X0); // Pop the value into X0
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
        var value = c.PopObject(Register.X0);   

        if (value.Type == StackObject.StackObjectType.Int)
        {
            c.PrintInteger(Register.X0);
        }
        else if (value.Type == StackObject.StackObjectType.String)
        {
            c.PrintString(Register.X0);
        }
        else if (value.Type == StackObject.StackObjectType.Float)
        {
           
            c.Fmov(Register.X0, Register.D0);
            c.PrintFloat(Register.D0);
        }

        return null;
    }
    public override Object? VisitIdexpresion(LanguageParser.IdexpresionContext context)
    {
        var id = context.ID().GetText();
        var (offset, obj) = c.GetObject(id); // Get the object associated with the ID

        c.Mov(Register.X0, offset); // Move the value to X0
        c.Add(Register.X0, Register.SP, Register.X0); // Add the offset to the stack pointer

        c.Ldr(Register.X0, Register.X0); // Load the value into X0
        c.Push(Register.X0); // Push the value onto the stack

        var newObject = c.CloneObject(obj); // Clone the object
        newObject.Id = null; // Clear the ID
        c.PushObject(newObject); // Push the object onto the stack
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

        var IntObject = c.IntObject();
        c.PushConstant(IntObject, int.Parse(value)); // Push the object onto the stack
        return null;
    }

public override object? VisitMulDiv(LanguageParser.MulDivContext context)
{
    c.Comment("Operación de multiplicación o división");
    var operation = context.op.Text;

    // Visita operandos
    Visit(context.expr(0));
    Visit(context.expr(1));

    var right = c.PopObject(Register.X1);
    var left = c.PopObject(Register.X0);

    // detect float
    bool isFloat = left.Type == StackObject.StackObjectType.Float || right.Type == StackObject.StackObjectType.Float;
    var resultType = isFloat ? StackObject.StackObjectType.Float : StackObject.StackObjectType.Int;

    if (isFloat)
    {
        // Convierte operandos si es necesario
        if (left.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D0, Register.X0);
        else
            c.Fmov(Register.D0, Register.X0);

        if (right.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D1, Register.X1);
        else
            c.Fmov(Register.D1, Register.X1);

        // Aplica operación flotante
        if (operation == "*")
            c.Fmul(Register.D0, Register.D0, Register.D1);
        else if (operation == "/")
            c.Fdiv(Register.D0, Register.D0, Register.D1);

        c.instructions.Add("STR D0, [SP, #-8]!"); // Push float
    }
    else
    {
        // Aplica operación entera
        if (operation == "*")
            c.Mul(Register.X0, Register.X0, Register.X1);
        else if (operation == "/")
            c.Div(Register.X0, Register.X0, Register.X1);

        c.Push(Register.X0);
    }

    // Empuja objeto resultante a la pila virtual
    var resultObject = new StackObject
    {
        Type = resultType,
        Length = 8,
        Depth = c.CurrentDepth,
        Id = null
    };
    c.PushObject(resultObject);

    return null;
}

public override object? VisitAddSub(LanguageParser.AddSubContext context)
{
    c.Comment("Operación de suma o resta");
    var operation = context.op.Text;

    Visit(context.expr(0));
    Visit(context.expr(1));

    var right = c.PopObject(Register.X1);
    var left = c.PopObject(Register.X0);

    bool isFloat = left.Type == StackObject.StackObjectType.Float || right.Type == StackObject.StackObjectType.Float;
    var resultType = isFloat ? StackObject.StackObjectType.Float : StackObject.StackObjectType.Int;

    if (isFloat)
    {
        if (left.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D0, Register.X0);
        else
            c.Fmov(Register.D0, Register.X0);

        if (right.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D1, Register.X1);
        else
            c.Fmov(Register.D1, Register.X1);

        if (operation == "+")
            c.Fadd(Register.D0, Register.D0, Register.D1);
        else if (operation == "-")
            c.Fsub(Register.D0, Register.D0, Register.D1);

        //  Sincroniza D0 → X0 para que print_float funcione correctamente
        c.Fmov(Register.X0, Register.D0);

        //  Guarda resultado flotante en la pila
        c.instructions.Add("STR D0, [SP, #-8]!");
    }
    else
    {
        if (operation == "+")
            c.Add(Register.X0, Register.X0, Register.X1);
        else if (operation == "-")
            c.Sub(Register.X0, Register.X0, Register.X1);

        c.Push(Register.X0);
    }

    var resultObject = new StackObject
    {
        Type = resultType,
        Length = 8,
        Depth = c.CurrentDepth,
        Id = null
    };
    c.PushObject(resultObject);

    return null;
}

    public override Object? VisitMod(LanguageParser.ModContext context)
    {
    c.Comment("Operación módulo (int % int)");

    Visit(context.expr(0)); // lado izquierdo
    Visit(context.expr(1)); // lado derecho

    var right = c.PopObject(Register.X1);
    var left = c.PopObject(Register.X0);

    // Validación de tipos
    if (left.Type != StackObject.StackObjectType.Int || right.Type != StackObject.StackObjectType.Int)
        throw new Exception("Error semántico: El operador % solo se puede aplicar entre enteros (int)");

    // x0 = x0 % x1
    // Proceso:
    //   q = x0 / x1
    //   q = q * x1
    //   r = x0 - q

    c.Div("x9", Register.X0, Register.X1); // x9 = x0 / x1 (cociente)
    c.Mul("x9", "x9", Register.X1);        // x9 = x9 * x1
    c.Sub(Register.X0, Register.X0, "x9"); // x0 = x0 - x9 (residuo)

    c.Push(Register.X0);

    var result = c.IntObject(); // resultado es de tipo int
    c.PushObject(result);

    return null;

    }

    public override Object? VisitIncremento(LanguageParser.IncrementoContext context)
    {
        return null;
    }

    public override Object? VisitDeclaracionVar(LanguageParser.DeclaracionVarContext context)
    {
        var varName = context.ID().GetText();
        c.Comment($"Variable declaration: {varName}");

        Visit(context.expr());
        c.TagObject(varName); // Tag the object with the variable name


        return null;
    }

    public override Object? VisitDeclaracionImplicita(LanguageParser.DeclaracionImplicitaContext context)
    {
        return null;
    }

    public override Object? VisitAsignarVar(LanguageParser.AsignarVarContext context)
    {
        var assignee = context.expr(0);
        if (assignee is LanguageParser.IdexpresionContext idContext)
        {
            string varName = idContext.ID().GetText();
            c.Comment($"Assigning value to variable: {varName}");
            Visit(context.expr(1)); // Visit the expression to get the value to assign
            var valueObject = c.PopObject(Register.X0); // Pop the value to assign
            var (offset, varObject) = c.GetObject(varName); // Get the variable object
            c.Mov(Register.X1, offset); // Move the value to the variable's offset
            c.Add(Register.X1, Register.SP, Register.X1); // Add the offset to the stack pointer
            c.Str(Register.X0, Register.X1); // Store the value in the variable's location

            varObject.Type = valueObject.Type; // Update the variable's type

            c.Push(Register.X0); // Push the value back onto the stack
            c.PushObject(c.CloneObject(varObject)); // Push the variable object back onto the stack
        }
        return null;
    }

    public override Object? VisitAsignarSliceCompleto(LanguageParser.AsignarSliceCompletoContext context)
    {
        return null;
    }

     public override object? VisitFloat(LanguageParser.FloatContext context)
    {
        var valueText = context.FLOAT().GetText();
        var value = double.Parse(valueText, CultureInfo.InvariantCulture);

        c.Comment($"Constante float: {value}");

        var floatObj = c.FloatObject();
        c.PushConstant(floatObj, value);

        return null;
    }


    public override object? VisitBoleanTrueExpresion(LanguageParser.BoleanTrueExpresionContext context)
    {
        c.Comment("Constante booleana: true");

        var boolObj = c.IntObject();
        c.PushConstant(boolObj, 1);

        return null;
    }

    public override object? VisitBoleanFalseExpresion(LanguageParser.BoleanFalseExpresionContext context)
    {
        c.Comment("Constante booleana: false");

        var boolObj = c.IntObject();
        c.PushConstant(boolObj, 0);

        return null;
    }


    public override Object? VisitCadenaExpresion(LanguageParser.CadenaExpresionContext context)
    {
        var value = context.CADENA().GetText().Trim('"');
        c.Comment($"String constant: {value}");
        var stringObject = c.StringObject();
        c.PushConstant(stringObject, value); // Push the string object onto the stack

        return null;
    }

    public override Object? VisitCaracterExpresion(LanguageParser.CaracterExpresionContext context)
    {
        return null;
    }

    public override object? VisitOperadorNegacion(LanguageParser.OperadorNegacionContext context)
    {
        Visit(context.right);

        var value = c.PopObject(Register.X0);

        if (value.Type != StackObject.StackObjectType.Int)
            throw new Exception("La negación lógica (!) solo se puede aplicar a booleanos representados como enteros");

        c.Cmp(Register.X0, "0");
        c.Cset(Register.X0, "EQ");

        c.Push(Register.X0);
        c.PushObject(c.IntObject());

        return null;
    }


public override object? VisitRelational(LanguageParser.RelationalContext context)
{
    var op = context.op.Text;
    c.Comment($"Operación relacional: {op}");

    Visit(context.expr(0));
    Visit(context.expr(1));

    var right = c.PopObject(Register.X1);
    var left = c.PopObject(Register.X0);

    bool isFloat = left.Type == StackObject.StackObjectType.Float || right.Type == StackObject.StackObjectType.Float;

    if (isFloat)
    {
        if (left.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D0, Register.X0);
        else
            c.Fmov(Register.D0, Register.X0);

        if (right.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D1, Register.X1);
        else
            c.Fmov(Register.D1, Register.X1);

        c.Fcmp(Register.D0, Register.D1);
    }
    else
    {
        c.Cmp(Register.X0, Register.X1);
    }

    string condition = op switch
    {
        ">"  => "GT",
        "<"  => "LT",
        ">=" => "GE",
        "<=" => "LE",
        _ => throw new Exception($"Operador relacional no soportado: {op}")
    };

    c.Cset(Register.X0, condition);

    c.Push(Register.X0);
    c.PushObject(c.IntObject());

    return null;
}

public override object? VisitEqualitys(LanguageParser.EqualitysContext context)
{
    var op = context.op.Text;

    Visit(context.expr(0));
    Visit(context.expr(1));

    var right = c.PopObject(Register.X1);
    var left = c.PopObject(Register.X0);

    bool isFloat = left.Type == StackObject.StackObjectType.Float || right.Type == StackObject.StackObjectType.Float;

    if (isFloat)
    {
        if (left.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D0, Register.X0);
        else
            c.Fmov(Register.D0, Register.X0);

        if (right.Type == StackObject.StackObjectType.Int)
            c.Scvtf(Register.D1, Register.X1);
        else
            c.Fmov(Register.D1, Register.X1);

        c.instructions.Add($"FCMP {Register.D0}, {Register.D1}");
    }
    else
    {
        c.instructions.Add($"CMP {Register.X0}, {Register.X1}");
    }

    string condition = op == "==" ? "EQ" : "NE";

    c.instructions.Add($"CSET {Register.X0}, {condition}");

    c.Push(Register.X0);
    c.PushObject(c.IntObject());

    return null;
}


    public override Object? VisitIncrementoDecremento(LanguageParser.IncrementoDecrementoContext context)
    {
        return null;
    }

    public override object? VisitLogicos(LanguageParser.LogicosContext context)
    {
        var op = context.op.Text;

        Visit(context.expr(0));
        Visit(context.expr(1));

        var right = c.PopObject(Register.X1);
        var left = c.PopObject(Register.X0);

        if (left.Type != StackObject.StackObjectType.Int || right.Type != StackObject.StackObjectType.Int)
            throw new Exception("El operador lógico solo puede aplicarse a booleanos (int 0 o 1)");

        if (op == "&&")
            c.instructions.Add($"AND {Register.X0}, {Register.X0}, {Register.X1}");
        else if (op == "||")
            c.instructions.Add($"ORR {Register.X0}, {Register.X0}, {Register.X1}");
        else
            throw new Exception($"Operador lógico no soportado: {op}");

        c.Push(Register.X0);
        c.PushObject(c.IntObject());

        return null;
    }

    public override Object? VisitBloqueInstrucciones(LanguageParser.BloqueInstruccionesContext context)
    {

        c.Comment("Block of instructions");
        c.NewScope(); // Create a new scope

        foreach (var instruccion in context.listainstrucciones())
        {
            Visit(instruccion); // Visit each instruction in the block
        }

        int bytesToRemove = c.EndScope(); // End the scope and get the number of bytes to remove

        if (bytesToRemove > 0)
        {
            c.Comment($"Removing {bytesToRemove} bytes from the stack");
            c.Mov(Register.X0, bytesToRemove); // Move the number of bytes to remove to X0
            c.Add(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer
            c.Comment($"Stack pointer adjusted by {bytesToRemove} bytes");
        }
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

