
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

public class FunctionMetadata
{
    public int FrameSize;
    public StackObject.StackObjectType ReturnType;
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
    
    private Dictionary<string, FunctionMetadata> functions = new Dictionary<string, FunctionMetadata>();
    private String continueLabel = "";
    private String breakLabel = "";
    private String returnLabel = "";
    
    private String? insideFunction = null;
    private int framePointerOffset = 0;
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
        var isDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
        var value = c.PopObject(isDouble ? Register.D0 : Register.X0); // Pop the value into X0 or D0   

        if (value.Type == StackObject.StackObjectType.Int)
        {
            c.PrintInteger(Register.X0);
        }
        else if (value.Type == StackObject.StackObjectType.Float)
        {
            c.PrintFloat();
        }
        else if (value.Type == StackObject.StackObjectType.String)
        {
            c.PrintString(Register.X0);
        }
        
        c.PrintNewline();


        return null;
    }

    
    public override Object? VisitIdexpresion(LanguageParser.IdexpresionContext context)
    {
        var id = context.ID().GetText();
        var (offset, obj) = c.GetObject(id); // Get the object associated with the ID

        if(insideFunction != null)
        {
            c.Mov(Register.X0, obj.Offset * 8); // Move the offset to X0 
            c.Sub(Register.X0, Register.FP, Register.X0); // Calculate the address in the stack frame
            c.Ldr(Register.X0, Register.X0); // Load the value into X0
            c.Push(Register.X0); // Push the value onto the stack
            var CloneObject = c.CloneObject(obj); // Clone the object
            CloneObject.Id = null; // Clear the ID
            c.PushObject(CloneObject); // Push the object onto the stack
            return null;
        }
       


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

    // Visita los operandos
    Visit(context.expr(0));
    Visit(context.expr(1));

    // Obtiene los operandos de la pila
    var isRightDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var right = c.PopObject(isRightDouble ? Register.D0 : Register.X0);
    var isLeftDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var left = c.PopObject(isLeftDouble ? Register.D1 : Register.X1);

    // Manejo de operaciones con flotantes
    if (isLeftDouble || isRightDouble)
    {
        if (!isLeftDouble) c.Scvtf(Register.D1, Register.X1); // Convierte entero a flotante
        if (!isRightDouble) c.Scvtf(Register.D0, Register.X0); // Convierte entero a flotante

        if (operation == "+")
        {
            c.Fadd(Register.D0, Register.D0, Register.D1); // Suma flotante
        }
        else if (operation == "-")
        {
            c.Fsub(Register.D0, Register.D0, Register.D1); // Resta flotante
        }

        c.Comment("Pushing float result");
        c.Push(Register.D0);
        c.PushObject(c.CloneObject(isLeftDouble ? left : right));
    }
    else
    {
        // Manejo de operaciones con enteros
        if (operation == "+")
        {
            c.Add(Register.X0, Register.X0, Register.X1); // Suma entera
        }
        else if (operation == "-")
        {
            c.Sub(Register.X0, Register.X1, Register.X0); // left - right ✅
        }


        c.Comment("Pushing integer result");
        c.Push(Register.X0);

        // Empuja el objeto resultante a la pila virtual
        var resultObject = new StackObject
        {
            Type = StackObject.StackObjectType.Int,
            Length = 8,
            Depth = c.CurrentDepth,
            Id = null
        };
        c.PushObject(resultObject);
    }

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

    // public override Object? VisitIncremento(LanguageParser.IncrementoContext context)
    // {
    //     return null;
    // }
public override Object? VisitVariables(LanguageParser.VariablesContext context)
{
    var varName = context.ID().GetText();
    c.Comment($"Variable declaration: {varName}");

    Visit(context.expr()); // Evalúa la expresión de inicialización

    if (insideFunction != null)
    {
        var localObject = c.GetFrameLocal(framePointerOffset);
        var valueObject = c.PopObject(Register.X0); // Obtiene el valor de la variable

        //localobject puede ser
        c.Mov(Register.X1, framePointerOffset * 8); // Mueve el offset al registro X1
        c.Sub(Register.X1, Register.FP, Register.X1); // Calcula la dirección en la pila
        c.Str(Register.X0, Register.X1); // Almacena el valor en la dirección calculada

        localObject.Type = valueObject.Type; // Actualiza el tipo del objeto en la pila
        framePointerOffset++; // Incrementa el offset para la siguiente variable

        return null;


    }
    // Registra la variable en la pila virtual
    c.TagObject(varName);
    c.Comment($"Variable {varName} registered in stack");

    return null;
}

public override Object? VisitAssign(LanguageParser.AssignContext context)
{
    var assignee = context.ID();

    if (assignee is LanguageParser.IdexpresionContext idContext)
    
    {
        c.Comment($"Assignment to variable: {assignee}");

        string varName = idContext.ID().GetText();
        Visit(context.expr()); // Evalúa la expresión de asignación

        var valueObject = c.PopObject(Register.X0); // Obtiene el valor a asignar

        var (offset, varObject) = c.GetObject(varName);

        if (insideFunction != null)
        {
            c.Mov(Register.X1, varObject.Offset * 8); // Mueve el offset al registro X1
            c.Sub(Register.X1, Register.FP, Register.X1); // Calcula la dirección en la pila
            c.Str(Register.X0, Register.X1); // Almacena el valor en la dirección calculada
            return null;
        }

        c.Mov(Register.X1, offset); // Mueve el offset al registro X1
        c.Add(Register.X1, Register.SP, Register.X1); // Calcula la dirección en la pila
        c.Str(Register.X0, Register.X1); // Almacena el valor en la dirección calculada

        // Actualiza el tipo del objeto en la pila
        varObject.Type = valueObject.Type;

        c.Push(Register.X0);
        c.PushObject(c.CloneObject(varObject));
    }
    return null;
}


    // public override Object? VisitAsignarSliceCompleto(LanguageParser.AsignarSliceCompletoContext context)
    // {
    //     return null;
    // }

     public override object? VisitFloat(LanguageParser.FloatContext context)
    {
        var value = context.FLOAT().GetText();

        var FloatObject = c.FloatObject();
        c.PushConstant(FloatObject, double.Parse(value)); // Push the float object onto the stack

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

    var isRightDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var right = c.PopObject(isRightDouble ? Register.D0 : Register.X0);
    var isLeftDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var left = c.PopObject(isLeftDouble ? Register.D1 : Register.X1);

    if (isLeftDouble || isRightDouble)
    {
        //TODO:
    }
    c.Cmp(Register.X1, Register.X0);
    var trueLabel = c.GetLabel();
    var endLabel = c.GetLabel();
    switch (op)
    {
        case "<":
            c.Blt(trueLabel);
            break;
        case "<=":
            c.Ble(trueLabel);
            break;
        case ">":
            c.Bgt(trueLabel);
            break;
        case ">=": 
            c.Bge(trueLabel);
            break;
        case "==":
            c.Beq(trueLabel);
            break;
        case "!=":
            c.Bne(trueLabel);
            break;
    }
    c.Mov(Register.X0, 0);
    c.Push(Register.X0);
    c.B(endLabel);
    c.SetLabel(trueLabel);
    c.Mov(Register.X0, 1);
    c.Push(Register.X0);
    c.SetLabel(endLabel);

    c.PushObject(c.BoolObject());
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
        c.Comment("If statement");
        Visit(context.expr());
        c.PopObject(Register.X0); // Pop the value to check the condition
        
        

        var hasElse = context.instruccion().Length > 1;

        if (hasElse)
        {
            c.Comment("If-else statement");
            var elseLabel = c.GetLabel();
            var endLabel = c.GetLabel();

            c.Cbz(Register.X0, elseLabel); // Check if the condition is false
            Visit(context.instruccion(0)); // Visit the first instruction (if block)
            c.B(endLabel); // Jump to the end label
            c.SetLabel(elseLabel); // Set the else label
            Visit(context.instruccion(1)); // Visit the second instruction (else block)
            c.SetLabel(endLabel); // Set the end label
        }
        else
        {
            c.Comment("If statement");
            var endLabel = c.GetLabel();
            c.Cbz(Register.X0, endLabel); // Check if the condition is false
            Visit(context.instruccion(0)); // Visit the instruction (if block)
            c.SetLabel(endLabel); // Set the end label
        }

        
        return null;
    }

    public override object VisitBoolean(LanguageParser.BooleanContext context)
    {
        var value = context.BOOL().GetText();

        c.Comment($"Boolean constant: {value}");
        var boolObject = c.BoolObject();
        c.PushConstant(boolObject, value == "true" ? true : false); // Push the boolean object onto the stack
        return null;
    }

    public override Object? VisitWhileInstruccion(LanguageParser.WhileInstruccionContext context)
    {
        c.Comment("While statement");
        var startLabel = c.GetLabel();
        var endLabel = c.GetLabel();

        var prevContinueLabel = continueLabel;
        var prevBreakLabel = breakLabel;
        continueLabel = startLabel; // Set the continue label
        breakLabel = endLabel; // Set the break label

        c.SetLabel(startLabel); // Set the start label
        Visit(context.expr()); // Visit the condition expression
        c.PopObject(Register.X0); // Pop the value to check the condition
        c.Cbz(Register.X0, endLabel); // Check if the condition is false
        Visit(context.instruccion()); // Visit the instruction block
        c.B(startLabel); // Jump back to the start label
        c.SetLabel(endLabel); // Set the end label

        c.Comment("End of while statement");

        continueLabel = prevContinueLabel; // Restore the previous continue label
        breakLabel = prevBreakLabel; // Restore the previous break label

        return null;
    }

    public override Object? VisitSwitchInstruccion(LanguageParser.SwitchInstruccionContext context)
    {
        return null;
    }

    // public override Object? VisitDeclaracionSlice(LanguageParser.DeclaracionSliceContext context)
    // {
    //     return null;
    // }

    // public override Object? VisitDeclaracionSlicevacio(LanguageParser.DeclaracionSlicevacioContext context)
    // {
    //     return null;
    // }

    // public override Object? VisitNilExpresion(LanguageParser.NilExpresionContext context)
    // {
    //     return null;
    // }

    // public override Object? VisitDeclaracionSlicemulti(LanguageParser.DeclaracionSlicemultiContext context)
    // {
    //     return null;
    // }

    public override Object? VisitAccesoSlice(LanguageParser.AccesoSliceContext context)
    {
        return null;
    }

    // public override Object? VisitAsignarSlice(LanguageParser.AsignarSliceContext context)
    // {
    //     return null;
    // }

    // public override Object? VisitAsignarSliceMulti(LanguageParser.AsignarSliceMultiContext context)
    // {
    //     return null;
    // }

    public override Object? VisitAccesoSliceMulti(LanguageParser.AccesoSliceMultiContext context)
    {
        return null;
    }

    // public override Object? VisitForCondicion(LanguageParser.ForCondicionContext context)
    // {
    // }


    public override Object? VisitForincicializacion(LanguageParser.ForincicializacionContext context)
    {
        var startLabel = c.GetLabel();
        var endLabel = c.GetLabel();
        var incrementLabel = c.GetLabel();  

        var prevContinueLabel = continueLabel;
        var prevBreakLabel = breakLabel;

        continueLabel = incrementLabel; // Set the continue label
        breakLabel = endLabel; // Set the break label

        c.Comment("For loop");
        c.NewScope(); // Create a new scope

        Visit(context.forInit()); // Visit the initialization expression
        c.SetLabel(startLabel); // Set the start label
        Visit(context.expr(0)); // Visit the condition expression
        c.PopObject(Register.X0); // Pop the value to check the condition
        c.Cbz(Register.X0, endLabel); // Check if the condition is false

        Visit(context.instruccion()); // Visit the instruction block
        c.SetLabel(incrementLabel); // Set the increment label
        Visit(context.expr(1)); // Visit the increment expression
        c.B(startLabel); // Jump back to the start label
        c.SetLabel(endLabel); // Set the end label

        c.Comment("End of for loop");
        var bytesToRemove = c.EndScope(); // End the scope and get the number of bytes to remove

        if (bytesToRemove > 0)
        {
            c.Mov(Register.X0, bytesToRemove); // Move the number of bytes to remove to X0
            c.Add(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer
        }
        continueLabel = prevContinueLabel; // Restore the previous continue label
        breakLabel = prevBreakLabel; // Restore the previous break label
        return null;

    }

    // public override Object? VisitForRange(LanguageParser.ForRangeContext context)
    // {
    //     return null;
    // }

    public override Object? VisitBreakInstruccion(LanguageParser.BreakInstruccionContext context)
    {
        c.Comment("Break statement");
        if (breakLabel != null)
        {
            c.B(breakLabel); // Jump to the break label
        }
        return null;
    }

    public override Object? VisitReturnInstruccion(LanguageParser.ReturnInstruccionContext context)
    {
        c.Comment("Return statement");
        if (context.expr() == null)
        {
            c.Br(returnLabel); // Jump to the return label
            return null;

        }
        
        if (insideFunction == null)
        {
            throw new Exception("Error semántico: La instrucción return solo se puede usar dentro de una función");
        }

        Visit(context.expr()); // Visit the return expression
        c.PopObject(Register.X0); // Pop the return value into X0

        c.Comment("Storing return value");
        var frameSize = functions[insideFunction].FrameSize; // Tamaño del marco de pila
        var returnOffset = frameSize - 1; // Desplazamiento para el valor de retorno
        c.Mov(Register.X1, returnOffset * 8); // Calcula el desplazamiento
        c.Sub(Register.X1, Register.FP, Register.X1); // Dirección en el marco de pila
        c.Str(Register.X0, Register.X1); // Almacena el valor de retorno

        c.Comment("Jumping to return label");
        c.B(returnLabel); // Salta a la etiqueta de retorno

        return null;
    }

    public override Object? VisitContinueInstruccion(LanguageParser.ContinueInstruccionContext context)
    {
        c.Comment("Continue statement");
        if (continueLabel != null)
        {
            c.B(continueLabel); // Jump to the continue label
        }
        
        return null;
    }

    public override Object? VisitCallee(LanguageParser.CalleeContext context)
    {
        if (context.expr() is not LanguageParser.IdexpresionContext idContext) return null;

        string funcName = idContext.ID().GetText();
        var call = context.call()[0];

        if (call is not LanguageParser.FuncCallContext callContext) return null;

        //TODO embeded

        var postFuncCallLabel = c.GetLabel(); // Get the label for the post function call

        int baseOffset = 2;
        int stackelementSize = 8;

        c.Mov(Register.X0, baseOffset * stackelementSize); // Move the base offset to X0
        c.Sub(Register.SP, Register.SP, Register.X0); // Calculate the address in the stack frame

        if (callContext.args() != null)
        {
            foreach (var arg in callContext.args().expr())
            {
                Visit(arg); // Visit each argument expression
            }
        }

        c.Comment($"Calling function {funcName}");

        c.Mov(Register.X0, stackelementSize * (baseOffset + callContext.args().expr().Length)); // Move the number of arguments to X0
        c.Add(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer

        c.Mov(Register.X0, stackelementSize);
        c.Sub(Register.X0, Register.SP, Register.X0); // Calculate the address in the stack frame

        c.Adr(Register.X1, postFuncCallLabel); // Load the address into X0
        c.Push(Register.X1); // Push the address onto the stack

        c.Push(Register.FP); // Push the frame pointer onto the stack
        c.Add(Register.FP, Register.X0, Register.XZR); // Set the frame pointer to the current stack pointer

        int frameSize = functions[funcName].FrameSize; // Get the frame size of the function
        c.Mov(Register.X0, (frameSize -2) * stackelementSize); // Move the frame size to X0
        c.Sub(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer

        c.Comment($"Calling function {funcName}");
        c.Bl(funcName); // Branch to the function  
        c.Comment($"Function {funcName} returned");
        c.SetLabel(postFuncCallLabel); // Set the post function call label

        var returnOffset = frameSize - 1; // Get the return offset
        c.Mov(Register.X4, returnOffset * stackelementSize);
        c.Sub(Register.X4, Register.FP, Register.X4); // Calculate the address in the stack frame
        c.Ldr(Register.X4, Register.X4); // Load the return value into X0

        c.Mov(Register.X1, stackelementSize);
        c.Sub(Register.X1, Register.FP, Register.X1); // Calculate the address in the stack frame
        c.Ldr(Register.FP, Register.X1); // Load the return address into X0

        c.Mov(Register.X0, stackelementSize * frameSize); // Move the frame size to X0
        c.Add(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer

        c.Push(Register.X4); // Push the return value onto the stack
        c.PushObject(new StackObject
        {
            Type = functions[funcName].ReturnType,
            Id = null,
            Offset = 0,
            Length = 8,
        }); // Push the return object onto the stack

        return null;
    }

public override Object? VisitFuncdlc(LanguageParser.FuncdlcContext context)
    {

        int baseOffset = 2;
        int paramsOffset = 0;

        if (context.@params() != null)
        {
            paramsOffset = context.@params().param().Length;
        }

        FrameVisitor frameVisitor = new FrameVisitor(baseOffset + paramsOffset);

        foreach (var listainstrucciones in context.listainstrucciones())
        {
            frameVisitor.Visit(listainstrucciones); // Visit each declaration in the function
        }

        var frame = frameVisitor.Frame;
        int localOffset = frame.Count;
        int returnOffset = 1;

        int totalFrameSize = baseOffset + paramsOffset + localOffset + returnOffset;

        string funcName = context.ID().GetText();
        StackObject.StackObjectType funcType = GetType(context.tipo().GetText());

        functions.Add(funcName, new FunctionMetadata
        {
            FrameSize = totalFrameSize,
            ReturnType = funcType
        });

        var prevInstruction = c.instructions;
        c.instructions = new List<string>();


        var paramCount = 0;
        foreach (var param in context.@params().param())
        {
            c.PushObject(new StackObject
            {
                Type = GetType(param.tipo().GetText()),
                Id = param.ID().GetText(),
                Offset = paramCount + baseOffset,
                Length = 8,
            });
            paramCount++;
        }

        foreach (FrameElement element in frame)
        {
            c.PushObject(new StackObject
            {
                Type = StackObject.StackObjectType.Undefined,
                Id = element.Name,
                Offset = element.Offset,
                Length = 8,
            });

        }

        insideFunction = funcName;
        framePointerOffset = 0;

        returnLabel = c.GetLabel(); // Get the return label

        c.SetLabel(funcName); // Set the function label

        foreach (var listainstrucciones in context.listainstrucciones())
        {
            Visit(listainstrucciones); // Visit each instruction in the function
        }

        c.SetLabel(returnLabel); // Set the return label

        c.Add(Register.X0, Register.FP, Register.XZR); // Add the stack pointer to the return address
        c.Ldr(Register.LR, Register.X0); // Load the return address into X0
        c.Br(Register.LR); // Branch to the return address

        c.Comment($"Function {funcName} frame size: {totalFrameSize} bytes");

        for (int i = 0; i < paramsOffset + localOffset; i++)
        {
            c.PopObject(); // Pop the parameter from the stack
        }

        foreach (var instruccion in c.instructions)
        {
            c.funcInstructions.Add(instruccion); // Add the instruction to the function instructions
        }

        c.instructions = prevInstruction; // Restore the previous instruction list
        insideFunction = null; // Clear the inside function variable
        c.Comment($"End of function {funcName}");


        return null;
    }

    StackObject.StackObjectType GetType(string name)
    {
        switch(name)
        {
            case "int":
                return StackObject.StackObjectType.Int;
            case "float":
                return StackObject.StackObjectType.Float;
            case "string":
                return StackObject.StackObjectType.String;
            case "bool":
                return StackObject.StackObjectType.Bool;
            default:
                throw new Exception($"Tipo desconocido: {name}");
        }

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

