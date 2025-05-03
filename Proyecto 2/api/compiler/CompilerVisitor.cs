
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
    public string output = "";

    public CompilerVisitor()
    {
    }

    public SymbolTable symbolTable = new SymbolTable();
        public static SymbolTable GlobalSymbolTable = new SymbolTable();

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
    else if (value.Type == StackObject.StackObjectType.Bool)
    {
        // Imprime el valor booleano como 1 (true) o 0 (false)
        c.Comment("Printing boolean value");
        c.PrintInteger(Register.X0); // Usa PrintInteger para imprimir el valor booleano
    }
    else if (value.Type == StackObject.StackObjectType.Char)
    {
        c.Comment("Printing character value");
        c.PrintChar(); // Usa la nueva función PrintChar que utiliza print_char
    }

    else
    {
        throw new Exception($"Unsupported type for printing: {value.Type}");
    }
    
    c.PrintNewline(); // Imprime un salto de línea después de cada impresión

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
    Console.WriteLine("Visitando Parens");
    return Visit(context.expr());
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
    var right = c.TopObject();
    var rightType = right.Type;
    var rightReg = rightType == StackObject.StackObjectType.Float ? Register.D0 : Register.X0;
    right = c.PopObject(rightReg);

    var left = c.TopObject();
    var leftType = left.Type;
    var leftReg = leftType == StackObject.StackObjectType.Float ? Register.D1 : Register.X1;
    left = c.PopObject(leftReg);

    // Manejo especial para concatenación de strings
    if ((leftType == StackObject.StackObjectType.String || rightType == StackObject.StackObjectType.String) && operation == "+")
    {
        c.Comment("String concatenation operation");
        
        // Guardar la posición actual del heap antes de la concatenación
        c.Push(Register.HP);
        
        string copyFirstLabel = c.GetLabel(); // Etiqueta para copiar el primer string
        string copySecondLabel = c.GetLabel(); // Etiqueta para copiar el segundo string
        string continueFirstLabel = c.GetLabel(); // Etiqueta para continuar con el primer string
        string continueSecondLabel = c.GetLabel(); // Etiqueta para continuar con el segundo string
        string endLabel = c.GetLabel(); // Etiqueta para finalizar
        
        // Copiar el primer string (izquierdo) al heap
        if (leftType == StackObject.StackObjectType.String)
        {
            c.Comment("Copying first string to heap");
            c.Mov(Register.X2, Register.X1); // X2 = dirección del primer string
            c.SetLabel(copyFirstLabel);
            c.instructions.Add("LDRB w3, [x2]");
            c.Cbz("w3", copySecondLabel);
            // Guardar byte en el heap
            c.Strb("w3", Register.HP);
            // Incrementar punteros
            c.Add(Register.X2, Register.X2, "1");
            c.Add(Register.HP, Register.HP, "1");
            // Continuar con el siguiente byte
            c.B(copyFirstLabel);
        }
        
        c.SetLabel(copySecondLabel);
        if (rightType == StackObject.StackObjectType.String)
        {
            c.Comment("Copying second string to heap");
            c.Mov(Register.X2, Register.X0); // X2 = dirección del segundo string
            c.SetLabel(continueSecondLabel);
            c.instructions.Add("LDRB w3, [x2]");
            c.Cbz("w3", endLabel);
            // Guardar byte en el heap
            c.Strb("w3", Register.HP);
            // Incrementar punteros
            c.Add(Register.X2, Register.X2, "1");
            c.Add(Register.HP, Register.HP, "1");
            // Continuar con el siguiente byte
            c.B(continueSecondLabel);
        }
        
        // Añadir 0 al final del string resultante
        c.SetLabel(endLabel);
        c.Mov("w3", 0);
        c.Strb("w3", Register.HP);
        c.Add(Register.HP, Register.HP, "1");
        
        c.Pop(Register.X0);
        c.Push(Register.X0);
        c.PushObject(c.StringObject());
        
        return null;
    }
    
    else if (leftType == StackObject.StackObjectType.Float || rightType == StackObject.StackObjectType.Float)
    {
        if (leftType != StackObject.StackObjectType.Float) 
            c.Scvtf(Register.D1, Register.X1); 
        if (rightType != StackObject.StackObjectType.Float) 
            c.Scvtf(Register.D0, Register.X0); // Convierte entero a flotante

        if (operation == "+")
        {
            c.Comment("Float addition");
            c.Fadd(Register.D0, Register.D1, Register.D0); // left + right
        }
        else if (operation == "-")
        {
            c.Comment("Float subtraction");
            c.Fsub(Register.D0, Register.D1, Register.D0); // left - right
        }
        else if (operation == "*")
        {
            c.Comment("Float multiplication");
            c.Fmul(Register.D0, Register.D1, Register.D0); // left * right
        }
        else if (operation == "/")
        {
            c.Comment("Float division");
            c.Fdiv(Register.D0, Register.D1, Register.D0); // left / right
        }

        c.Push(Register.D0);
        c.PushObject(c.FloatObject());
    }
    else
    {
        // Manejo de operaciones con enteros
        if (operation == "+")
        {
            c.Comment("Integer addition");
            c.Add(Register.X0, Register.X1, Register.X0); // left + right
        }
        else if (operation == "-")
        {
            c.Comment("Integer subtraction");
            c.Sub(Register.X0, Register.X1, Register.X0); // left - right
        }
        else if (operation == "*")
        {
            c.Comment("Integer multiplication");
            c.Mul(Register.X0, Register.X1, Register.X0); // left * right
        }
        else if (operation == "/")
        {
            c.Comment("Integer division");
            c.Div(Register.X0, Register.X1, Register.X0); // left / right
        }

        c.Push(Register.X0);
        c.PushObject(c.IntObject());
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
public override Object? VisitDeclaracionVar(LanguageParser.DeclaracionVarContext context)
{
    var varName = context.ID().GetText();
    c.Comment($"Variable declaration: {varName}");

    StackObject valorPorDefecto;
    object valor;

    if (context.expr() != null)
    {
        Visit(context.expr()); // Evaluar la expresión de inicialización
        valorPorDefecto = c.TopObject(); // Ya fue empujado al stack virtual
    }
    else
    {
        var tipoTexto = context.tipo().GetText();

        switch (tipoTexto)
        {
            case "int":
                valorPorDefecto = c.IntObject();
                valor = 0;
                break;
            case "float64":
                valorPorDefecto = c.FloatObject();
                valor = 0.0;
                break;
            case "string":
                valorPorDefecto = c.StringObject();
                valor = "";
                break;
            case "bool":
                valorPorDefecto = c.BoolObject();
                valor = false;
                break;
            case "char":
            case "rune":
                      
            default:
                throw new Exception($"Tipo no soportado para inicialización: {tipoTexto}");
        }
        
        c.PushConstant(valorPorDefecto, valor);
    }

    if (insideFunction != null)
    {
        var localObject = c.GetFrameLocal(framePointerOffset);
        var valueObject = c.PopObject(Register.X0);

        c.Mov(Register.X1, framePointerOffset * 8);
        c.Sub(Register.X1, Register.FP, Register.X1);
        c.Str(Register.X0, Register.X1);

        localObject.Type = valueObject.Type;
        framePointerOffset++;

        return null;
    }

    c.TagObject(varName);
    c.Comment($"Variable {varName} registered in stack");
        GlobalSymbolTable.AddSymbol(varName, "Variable", valorPorDefecto.Type.ToString(), "Global", context.Start.Line, context.Start.Column);

    return null;
}


public override Object? VisitDeclaracionImplicita(LanguageParser.DeclaracionImplicitaContext context)
{
    var varName = context.ID().GetText();
    c.Comment($"Variable declaration: {varName}");

    Visit(context.expr());

    if (insideFunction != null)
    {
        var localObject = c.GetFrameLocal(framePointerOffset);
        var valueObject = c.PopObject(Register.X0);

        c.Mov(Register.X1, framePointerOffset * 8);
        c.Sub(Register.X1, Register.FP, Register.X1);
        c.Str(Register.X0, Register.X1);

        localObject.Type = valueObject.Type;
        framePointerOffset++;

        return null;
    }

    c.TagObject(varName);
    c.Comment($"Variable {varName} registered in stack");

    return null;
}

public override Object? VisitAssign(LanguageParser.AssignContext context)
{
    var varName = context.ID().GetText(); // obtiene el nombre de la variable directamente
    c.Comment($"Assignment to variable: {varName}");

    Visit(context.expr()); // Evalúa la expresión de asignación

    var valueObject = c.PopObject(Register.X0); // obtiene el valor a asignar
    var (offset, varObject) = c.GetObject(varName); // busca en la pila virtual

    if (insideFunction != null)
    {
        c.Mov(Register.X1, varObject.Offset * 8); // offset de stack frame
        c.Sub(Register.X1, Register.FP, Register.X1); // calcula dirección relativa al FP
        c.Str(Register.X0, Register.X1); // almacena el valor
    }
    else
    {
        c.Mov(Register.X1, offset); // offset para SP
        c.Add(Register.X1, Register.SP, Register.X1); // dirección final
        c.Str(Register.X0, Register.X1); // almacena el valor
    }

    varObject.Type = valueObject.Type;
    //****
    c.Push(Register.X0);
    c.PushObject(c.CloneObject(varObject));

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
    // Get the character from the context
    string charLiteral = context.CARACTER().GetText();
    
    // Extract the character between the single quotes
    char character = charLiteral[1]; // Get the character at index 1 (between quotes)
    int charValue = (int)character;  // Convert to its ASCII/Unicode value
    
    c.Comment($"Creating character literal: '{character}' (ASCII: {charValue})");
    
    // Create character object
    var charObj = c.IntObject(); 
    charObj.Type = StackObject.StackObjectType.Char;
    
    // Push the constant to the stack
    c.PushConstant(charObj, charValue);
    
    return null;
}

public override Object? VisitOperadorNegacion(LanguageParser.OperadorNegacionContext context)
{
    Visit(context.right);

    var value = c.PopObject(Register.X0);

    if (value.Type == StackObject.StackObjectType.Int)
    {
        c.Cmp(Register.X0, "0");
        c.Cset(Register.X0, "EQ");
        c.Push(Register.X0);
        c.PushObject(c.BoolObject());
        return null;
    }

    if (value.Type == StackObject.StackObjectType.Bool)
    {
        c.Cmp(Register.X0, "0");
        c.Cset(Register.X0, "EQ");
        c.Push(Register.X0);
        c.PushObject(c.BoolObject());
        return null;
    }

    throw new Exception($"La negación lógica (!) solo se puede aplicar a booleanos o enteros 0/1, no a: {value.Type}");
}


public override object? VisitRelational(LanguageParser.RelationalContext context)
{
    var op = context.op.Text;
    Console.WriteLine($"==> VisitRelational: Operador recibido: {op}");

    Visit(context.expr(0));
    Visit(context.expr(1));

    var isRightDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var right = c.PopObject(isRightDouble ? Register.D0 : Register.X0);

    var isLeftDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var left = c.PopObject(isLeftDouble ? Register.D1 : Register.X1);

    //Console.WriteLine($"Relational operands: Left={left.Type}, Right={right.Type}");

    var trueLabel = c.GetLabel();
    var endLabel = c.GetLabel();

    if (isLeftDouble || isRightDouble)
    {
        c.Comment("Float comparison");
        
        if (left.Type == StackObject.StackObjectType.Int)
        {
            c.Comment("Converting left operand from int to float");
            c.Scvtf(Register.D1, Register.X1);
        }
        else if (left.Type == StackObject.StackObjectType.Float)
        {
            c.Comment("Left operand is already float");
            c.Fmov(Register.D1, Register.D1);
        }

        if (right.Type == StackObject.StackObjectType.Int)
        {
            c.Comment("Converting right operand from int to float");
            c.Scvtf(Register.D0, Register.X0);
        }
        else if (right.Type == StackObject.StackObjectType.Float)
        {
            c.Comment("Right operand is already float");
            c.Fmov(Register.D0, Register.D0);
        }

        // Realizar la comparación
        c.Comment($"Comparing D1 and D0 with {op}");
        c.Fcmp(Register.D1, Register.D0);

        switch (op)
        {
            case "<":
                c.Comment("Branch if less than");
                c.instructions.Add($"B.LT {trueLabel}");
                break;
            case "<=":
                c.Comment("Branch if less than or equal");
                c.instructions.Add($"B.LE {trueLabel}");
                break;
            case ">":
                c.Comment("Branch if greater than");
                c.instructions.Add($"B.GT {trueLabel}");
                break;
            case ">=":
                c.Comment("Branch if greater than or equal");
                c.instructions.Add($"B.GE {trueLabel}");
                break;
            default:
                throw new Exception($"Operador relacional no válido con float: {op}");
        }

       
        c.Comment("False case - set X0 to 0");
        c.Mov(Register.X0, 0);
        c.Push(Register.X0);
        c.B(endLabel);
        
       
        c.SetLabel(trueLabel);
        c.Comment("True case - set X0 to 1");
        c.Mov(Register.X0, 1);
        c.Push(Register.X0);
        
      
        c.SetLabel(endLabel);
        c.PushObject(c.BoolObject());
        
        Console.WriteLine($"==> Comparación float '{op}' ejecutada");
        return null;
    }

    c.Comment($"Integer comparison with {op}");
    c.Cmp(Register.X1, Register.X0);

    switch (op)
    {
        case "<":
            c.Comment("Branch if less than");
            c.Blt(trueLabel);
            break;
        case "<=":
            c.Comment("Branch if less than or equal");
            c.Ble(trueLabel);
            break;
        case ">":
            c.Comment("Branch if greater than");
            c.Bgt(trueLabel);
            break;
        case ">=":
            c.Comment("Branch if greater than or equal");
            c.Bge(trueLabel);
            break;
        default:
            throw new Exception($"Operador relacional no válido con int: {op}");
    }

    // Caso falso
    c.Comment("False case - set X0 to 0");
    c.Mov(Register.X0, 0);
    c.Push(Register.X0);
    c.B(endLabel);
    
    // Caso verdadero
    c.SetLabel(trueLabel);
    c.Comment("True case - set X0 to 1");
    c.Mov(Register.X0, 1);
    c.Push(Register.X0);
    
    // Final común
    c.SetLabel(endLabel);
    c.PushObject(c.BoolObject());
    
    Console.WriteLine($"==> Comparación int '{op}' ejecutada");
    return null;
}
public override object? VisitEqualitys(LanguageParser.EqualitysContext context)
{
    var op = context.op.Text;
    Console.WriteLine($"==> VisitEqualitys: {op}");

    Visit(context.expr(0));
    Visit(context.expr(1));

    // Verificamos si alguno de los operandos es float o string
    var isRightDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var isRightString = c.TopObject().Type == StackObject.StackObjectType.String;
    var isRightChar = c.TopObject().Type == StackObject.StackObjectType.Char;
    var right = c.PopObject(isRightDouble ? Register.D1 : Register.X1);

    var isLeftDouble = c.TopObject().Type == StackObject.StackObjectType.Float;
    var isLeftString = c.TopObject().Type == StackObject.StackObjectType.String;
    var isLeftChar = c.TopObject().Type == StackObject.StackObjectType.Char;
    var left = c.PopObject(isLeftDouble ? Register.D0 : Register.X0);

    Console.WriteLine($"Equality operands: Left={left.Type}, Right={right.Type}");

    // Declaramos etiquetas para todos los casos
    var trueLabel = c.GetLabel();
    var endLabel = c.GetLabel();

    // Case 1: Comparación con strings
    if (isLeftString && isRightString)
    {
        // Para comparar strings, necesitamos comparar cada carácter
        c.Comment("String comparison");
        
        // Implementar una función de comparación de strings
        var loopLabel = c.GetLabel();
        var charEqualLabel = c.GetLabel();
        var diffLabel = c.GetLabel();
        
        c.SetLabel(loopLabel);
        // Cargar un byte de cada string
        c.instructions.Add("LDRB W2, [X0]");
        c.instructions.Add("LDRB W3, [X1]");
        
        // Comparar bytes
        c.instructions.Add("CMP W2, W3");
        c.instructions.Add("BNE " + diffLabel);
        
        c.instructions.Add("CBZ W2, " + trueLabel); 
        
        // Avanzar al siguiente carácter
        c.instructions.Add("ADD X0, X0, #1");
        c.instructions.Add("ADD X1, X1, #1");
        c.instructions.Add("B " + loopLabel);
        
        // Si son diferentes, retornar false
        c.SetLabel(diffLabel);
        c.instructions.Add("MOV X0, #0");
        c.B(endLabel);
        
        c.SetLabel(trueLabel);
        c.instructions.Add("MOV X0, #1");
        
        // Punto final común
        c.SetLabel(endLabel);
        c.Push(Register.X0);
        c.PushObject(c.BoolObject());
        
        return null;
    }
    // Case 2: Comparación con floats
    else if (isLeftDouble || isRightDouble)
    {
        // Convertir ambos a float si es necesario
        if (!isLeftDouble) 
            c.Scvtf(Register.D0, Register.X0);
        
        if (!isRightDouble)
            c.Scvtf(Register.D1, Register.X1);
        
        c.Fcmp(Register.D0, Register.D1);
        
        if (op == "==")
            c.instructions.Add("B.EQ " + trueLabel);
        else // op == "!="
            c.instructions.Add("B.NE " + trueLabel);
        
        c.Mov(Register.X0, 0);
        c.B(endLabel);
        
        c.SetLabel(trueLabel);
        c.Mov(Register.X0, 1);
        
        c.SetLabel(endLabel);
        c.Push(Register.X0);
        c.PushObject(c.BoolObject());
        
        return null;
    }
    // Case 3: Comparación con caracteres
    else if (isLeftChar || isRightChar)
    {
        // Los caracteres se manejan como enteros 
        c.Cmp(Register.X0, Register.X1);
        
        if (op == "==")
            c.Beq(trueLabel);
        else // op == "!="
            c.Bne(trueLabel);
        
        c.Mov(Register.X0, 0);
        c.B(endLabel);
        
        c.SetLabel(trueLabel);
        c.Mov(Register.X0, 1);
        
        c.SetLabel(endLabel);
        c.Push(Register.X0);
        c.PushObject(c.BoolObject());
        
        return null;
    }
    // Case 4: Comparación normal con enteros o booleanos
    else
    {
        c.Cmp(Register.X0, Register.X1);
        
        string condition = op == "==" ? "EQ" : "NE";
        c.instructions.Add($"CSET {Register.X0}, {condition}");
        
        c.Push(Register.X0);
        c.PushObject(c.BoolObject());
        
        return null;
    }
}

public override object? VisitLogicos(LanguageParser.LogicosContext context)
{
    var op = context.op.Text;
    Console.WriteLine($"==> VisitLogicos: {op}");

    Visit(context.expr(0));
    Visit(context.expr(1));

    var right = c.PopObject(Register.X1);
    var left = c.PopObject(Register.X0);

    Console.WriteLine($"Logicos operands: Left={left.Type}, Right={right.Type}");

    var tiposValidos = new[] { StackObject.StackObjectType.Int, StackObject.StackObjectType.Bool };
    if (!tiposValidos.Contains(left.Type) || !tiposValidos.Contains(right.Type))
        throw new Exception("El operador lógico solo puede aplicarse a booleanos (int 0 o 1)");

    if (op == "&&")
        c.instructions.Add($"AND {Register.X0}, {Register.X0}, {Register.X1}");
    else if (op == "||")
        c.instructions.Add($"ORR {Register.X0}, {Register.X0}, {Register.X1}");
    else
        throw new Exception($"Operador lógico no soportado: {op}");

    c.Push(Register.X0);
    c.PushObject(c.BoolObject());

    Console.WriteLine($"==> Resultado lógico empujado como BoolObject");
    return null;
}

    
    
    
    
public override Object? VisitIncrementoDecremento(LanguageParser.IncrementoDecrementoContext context)
{
    var varName = context.ID().GetText();
    var op = context.GetText().EndsWith("++") ? "++" : "--";

    c.Comment($"Incremento/Decremento: {varName} {op}");

    var (offset, obj) = c.GetObject(varName);

    // Cargar valor actual
    if (insideFunction != null)
    {
        c.Mov(Register.X1, obj.Offset * 8);
        c.Sub(Register.X1, Register.FP, Register.X1);
        c.Ldr(Register.X0, Register.X1);
    }
    else
    {
        c.Mov(Register.X1, offset);
        c.Add(Register.X1, Register.SP, Register.X1);
        c.Ldr(Register.X0, Register.X1);
    }

    // Incrementar o decrementar
    if (op == "++")
        c.Add(Register.X0, Register.X0, "1");
    else
        c.Sub(Register.X0, Register.X0, "1");

    // Guardar valor actualizado
    if (insideFunction != null)
        c.Str(Register.X0, Register.X1);
    else
        c.Str(Register.X0, Register.X1);

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
    c.Comment("Switch statement");
    
    // Evaluar la expresión del switch
    Visit(context.expr());
    var switchValueReg = Register.X1;
    c.PopObject(switchValueReg);
    
    var endLabel = c.GetLabel(); // Etiqueta para salir del switch
    var prevBreakLabel = breakLabel;
    breakLabel = endLabel;
    
    var nextLabels = new List<string>(); // Lista de etiquetas para casos
    
    // Procesar cada caso
    foreach (var caseCtx in context.cases())
    {
        var nextLabel = c.GetLabel();
        nextLabels.Add(nextLabel);
        
        // Evaluar la expresión del case
        Visit(caseCtx.expr());
        var caseValueReg = Register.X0;
        c.PopObject(caseValueReg);
        
        c.Comment("Comparing switch value with case value");
        c.Cmp(switchValueReg, caseValueReg);
        c.Bne(nextLabel); // Si no son iguales, ir al siguiente caso
        
        c.Comment("Executing case block");
        foreach (var inst in caseCtx.listainstrucciones())
        {
            Visit(inst);
        }
        
        c.B(endLabel);
        
        // Etiqueta para el siguiente caso
        c.SetLabel(nextLabel);
    }
    
    // Procesar el caso default si existe
    if (context.defaultCase() != null)
    {
        c.Comment("Default case");
        foreach (var inst in context.defaultCase().listainstrucciones())
        {
            Visit(inst);
        }
    }
    
    // Etiqueta de fin del switch
    c.SetLabel(endLabel);
    
    breakLabel = prevBreakLabel; // Restaurar la etiqueta de break anterior
    
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

public override Object? VisitForCondicion(LanguageParser.ForCondicionContext context)
{
    var startLabel = c.GetLabel();
    var endLabel = c.GetLabel();

    var prevContinueLabel = continueLabel;
    var prevBreakLabel = breakLabel;

    continueLabel = startLabel; // El continue salta a evaluar la condición
    breakLabel = endLabel;      // El break salta al final del ciclo

    c.Comment("For-condición (como while)");
    c.NewScope();

    c.SetLabel(startLabel); // Etiqueta de inicio del ciclo
    Visit(context.expr());  // Evaluamos la condición
    c.PopObject(Register.X0); 
    c.Cbz(Register.X0, endLabel); // Si es falsa, salta al final

    Visit(context.instruccion()); // Ejecuta el cuerpo

    c.B(startLabel); // Regresa al inicio para reevaluar

    c.SetLabel(endLabel); // Marca el final del ciclo

    c.Comment("Fin del for-condición");
    var bytesToRemove = c.EndScope();
    if (bytesToRemove > 0)
    {
        c.Mov(Register.X0, bytesToRemove);
        c.Add(Register.SP, Register.SP, Register.X0);
    }

    continueLabel = prevContinueLabel;
    breakLabel = prevBreakLabel;

    return null;
}


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
    if (!string.IsNullOrEmpty(breakLabel))
    {
        c.B(breakLabel); // Saltar a la etiqueta de break
    }
    else
    {
        throw new Exception("Error semántico: 'break' fuera de un contexto válido");
    }
    return null;
}

    public override Object? VisitReturnInstruccion(LanguageParser.ReturnInstruccionContext context)
    {
        c.Comment("Return statement");
        if (context.expr() == null)
        {
            c.B(returnLabel); // Jump to the return label
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



// public override Object? VisitCallee(LanguageParser.CalleeContext context)
// {
//     Console.WriteLine("==> VisitCallee");
//     if (context.expr() is not LanguageParser.IdexpresionContext idContext) return null;

//     string funcName = idContext.ID().GetText();
//     var call = context.call()[0];

//     if (call is not LanguageParser.FuncCallContext callContext) return null;

//     //TODO embeded

//     var postFuncCallLabel = c.GetLabel(); // Get the label for the post function call

//     int baseOffset = 2;
//     int stackelementSize = 8;

//     c.Mov(Register.X0, baseOffset * stackelementSize); // Move the base offset to X0
//     c.Sub(Register.SP, Register.SP, Register.X0); // Calculate the address in the stack frame
//     Console.WriteLine($"Base offset: {baseOffset * stackelementSize}");
    
//     // Procesar argumentos
//     int argCount = 0;
//     if (callContext.args() != null)
//     {
//         foreach (var arg in callContext.args().expr())
//         {
//             Visit(arg); // Visit each argument expression
//         }
//         argCount = callContext.args().expr().Length;
//     }
//     Console.WriteLine($"Arguments count: {argCount}");
//     c.Comment($"Calling function {funcName}");

//     c.Mov(Register.X0, stackelementSize * (baseOffset + argCount));
//     c.Add(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer

//     c.Mov(Register.X0, stackelementSize);
//     c.Sub(Register.X0, Register.SP, Register.X0); // Calculate the address in the stack frame

//     c.Adr(Register.X1, postFuncCallLabel); // Load the address into X1
//     c.Push(Register.X1); // Push the address onto the stack

//     c.Push(Register.FP); // Push the frame pointer onto the stack
//     c.Add(Register.FP, Register.X0, Register.XZR); // Set the frame pointer to the current stack pointer

//     int frameSize = functions[funcName].FrameSize; // Get the frame size of the function
//     c.Mov(Register.X0, (frameSize - 2) * stackelementSize); // Move the frame size to X0
//     c.Sub(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer

//     c.Comment($"Calling function {funcName}");
//     c.Bl(funcName); // Branch to the function  
//     c.Comment($"Function {funcName} returned");
//     c.SetLabel(postFuncCallLabel); // Set the post function call label

//     var returnOffset = frameSize - 1; // Get the return offset
//     c.Mov(Register.X4, returnOffset * stackelementSize);
//     c.Sub(Register.X4, Register.FP, Register.X4); // Calculate the address in the stack frame
//     c.Ldr(Register.X4, Register.X4); // Load the return value into X4

//     // Restaurar el frame pointer
//     c.Mov(Register.X1, stackelementSize);
//     c.Sub(Register.X1, Register.FP, Register.X1); // Calculate the address in the stack frame
//     c.Ldr(Register.FP, Register.X1); // Load the frame pointer

//     // Ajustar el stack pointer para limpiar el frame
//     c.Mov(Register.X0, stackelementSize * frameSize); // Move the frame size to X0
//     c.Add(Register.SP, Register.SP, Register.X0); // Adjust the stack pointer

//     // Guardar el valor de retorno en la pila y crear el objeto correspondiente
//     c.Push(Register.X4); // Push the return value onto the stack
//     c.PushObject(new StackObject
//     {
//         Type = functions[funcName].ReturnType,
//         Id = null,
//         Offset = 0,
//         Length = 8,
//     }); // Push the return object onto the stack

//     return null;
// }



public override Object? VisitCallee(LanguageParser.CalleeContext context)
{
    Console.WriteLine("==> VisitCallee");
    if (context.expr() is not LanguageParser.IdexpresionContext idContext) return null;

    string funcName = idContext.ID().GetText();
    var call = context.call()[0];

    if (call is not LanguageParser.FuncCallContext callContext) return null;

    c.Comment($"Preparando llamada a función {funcName}");
    var postFuncCallLabel = c.GetLabel();

    int baseOffset = 2;
    int stackelementSize = 8;

    // Reservar espacio para el frame base
    c.Mov(Register.X0, baseOffset * stackelementSize);
    c.Sub(Register.SP, Register.SP, Register.X0);
    Console.WriteLine($"Base offset: {baseOffset * stackelementSize}");
    
    // Procesar y apilar argumentos
    int argCount = 0;
    if (callContext.args() != null)
    {
        foreach (var arg in callContext.args().expr())
        {
            Visit(arg);
        }
        argCount = callContext.args().expr().Length;
    }
    Console.WriteLine($"Arguments count: {argCount}");
    
    // Ajustar stack pointer después de argumentos
    c.Mov(Register.X0, stackelementSize * (baseOffset + argCount));
    c.Add(Register.SP, Register.SP, Register.X0);

    // Calcular dirección base de frame
    c.Mov(Register.X0, stackelementSize);
    c.Sub(Register.X0, Register.SP, Register.X0);

    // Guardar dirección de retorno y frame pointer anterior
    c.Adr(Register.X1, postFuncCallLabel);
    c.Push(Register.X1);
    c.Push(Register.FP);
    
    // Actualizar frame pointer
    c.Add(Register.FP, Register.X0, Register.XZR);

    // Reservar espacio para variables locales
    int frameSize = functions[funcName].FrameSize;
    c.Mov(Register.X0, (frameSize - 2) * stackelementSize);
    c.Sub(Register.SP, Register.SP, Register.X0);

    // Llamar a la función
    c.Comment($"Llamando a función {funcName}");
    c.Bl(funcName);
    
    c.Comment($"Función {funcName} ha retornado");
    c.SetLabel(postFuncCallLabel);

    // Recuperar valor de retorno
    var returnOffset = frameSize - 1;
    c.Mov(Register.X4, returnOffset * stackelementSize);
    c.Sub(Register.X4, Register.FP, Register.X4);
    c.Ldr(Register.X4, Register.X4);

    // Restaurar frame pointer anterior
    c.Mov(Register.X1, stackelementSize);
    c.Sub(Register.X1, Register.FP, Register.X1);
    c.Ldr(Register.FP, Register.X1);

    // Limpiar el frame completo
    c.Mov(Register.X0, stackelementSize * frameSize);
    c.Add(Register.SP, Register.SP, Register.X0);

    c.Mov(Register.X5, Register.XZR);
    c.Mov(Register.X6, Register.XZR); // Limpiar X6
    c.Mov(Register.X7, Register.XZR); // Limpiar X7

    // Apilar valor de retorno y crear objeto correspondiente
    c.Push(Register.X4);
    c.PushObject(new StackObject
    {
        Type = functions[funcName].ReturnType,
        Id = null,
        Offset = 0,
        Length = 8,
    });

    // Limpiar el registro de retorno
    c.Mov(Register.X4, Register.XZR);

    return null;
}


public override Object? VisitFuncdlc(LanguageParser.FuncdlcContext context)
{
    int baseOffset = 2;
    int paramsOffset = 0;
    Console.WriteLine("==> VisitFuncdlc");
    
    if (context.@params() != null)
    {
        Console.WriteLine("Function has parameters");
        paramsOffset = context.@params().param().Length;
        Console.WriteLine($"Function has {paramsOffset} parameters");
    }

    FrameVisitor frameVisitor = new FrameVisitor(baseOffset + paramsOffset);

    foreach (var listainstrucciones in context.listainstrucciones())
    {
        Console.WriteLine("Visiting function instructions");
        frameVisitor.Visit(listainstrucciones);
        Console.WriteLine("intrucciones visitadas");
    }

    var frame = frameVisitor.Frame;
    int localOffset = frame.Count;
    int returnOffset = 1;

    int totalFrameSize = baseOffset + paramsOffset + localOffset + returnOffset;

    string funcName = context.ID().GetText();
    Console.WriteLine($"Function name: {funcName}");
    
    StackObject.StackObjectType funcType = StackObject.StackObjectType.Void;
    if (context.tipo() != null)
    {
        funcType = GetType(context.tipo().GetText());
    }
    Console.WriteLine($"Function name: {funcName}, Type: {funcType}");
    
    functions.Add(funcName, new FunctionMetadata
    {
        FrameSize = totalFrameSize,
        ReturnType = funcType
    });

    var prevInstruction = c.instructions;
    c.instructions = new List<string>();

    Console.WriteLine($"Creating function {funcName}");
    
    // Procesar parámetros
    var paramCount = 0;
    if (context.@params() != null)
    {
        foreach (var param in context.@params().param())
        {
            Console.WriteLine($"Parameter {paramCount}: {param.ID().GetText()}");
            c.PushObject(new StackObject
            {
                Type = GetType(param.tipo().GetText()),
                Id = param.ID().GetText(),
                Offset = paramCount + baseOffset,
                Length = 8,
            });
            paramCount++;
        }
    }
    Console.WriteLine($"Function {funcName} has {paramCount} parameters");
    
    // Procesar variables locales
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

    returnLabel = c.GetLabel();

    // Definir punto de entrada de la función
    c.SetLabel(funcName);
    Console.WriteLine($"Function label: {funcName}");
    
    // Visitar instrucciones
    foreach (var listainstrucciones in context.listainstrucciones())
    {
        Visit(listainstrucciones);
    }

    // Punto de retorno
    c.SetLabel(returnLabel);

    c.Add(Register.X0, Register.FP, Register.XZR);
    c.Ldr(Register.LR, Register.X0);

    c.Mov(Register.X5, Register.XZR);
    c.Mov(Register.X6, Register.XZR);
    c.Mov(Register.X7, Register.XZR);
    
    // Retornar
    c.Br(Register.LR);

    c.Comment($"Function {funcName} frame size: {totalFrameSize} bytes");

    // Limpiar la pila
    for (int i = 0; i < paramsOffset + localOffset; i++)
    {
        c.PopObject();
    }

    foreach (var instruccion in c.instructions)
    {
        c.funcInstructions.Add(instruccion);
    }

    // Restaurar el estado
    c.instructions = prevInstruction;
    insideFunction = null;
    c.Comment($"End of function {funcName}");
            int line = context.Start.Line;
        int column = context.Start.Column;

    GlobalSymbolTable.AddSymbol(funcName, "Function", funcType.ToString(), "Global", line, column);

    return null;
}



private StackObject.StackObjectType GetType(string name)
{
    switch(name.ToLower())
    {
        case "int":
            return StackObject.StackObjectType.Int;
        case "float64":
            return StackObject.StackObjectType.Float;
        case "string":
            return StackObject.StackObjectType.String;
        case "bool":
            return StackObject.StackObjectType.Bool;
        case "char":
        case "rune":
            return StackObject.StackObjectType.Char;
        default:
            throw new Exception($"Tipo no soportado: {name}");
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



    public override Object? VisitInstanciaStruct(LanguageParser.InstanciaStructContext context)
    {
        return null;
    }
}

