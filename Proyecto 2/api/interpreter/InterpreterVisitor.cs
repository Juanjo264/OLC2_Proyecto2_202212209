
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

public override ValueWrapper VisitImpresiones(LanguageParser.ImpresionesContext context) {
    List<string> valores = new List<string>();

    void ObtenerValores(LanguageParser.ImpresionesContext ctx) {
        if (ctx.impresiones() != null) {
            ObtenerValores(ctx.impresiones());
        }

        ValueWrapper valor = Visit(ctx.expr());

        string valorStr = ObtenerValorComoString(valor);

        valores.Add(valorStr);
    }

    ObtenerValores(context);

    output += string.Join(" ", valores);

    return defaultValue;
}

private string ObtenerValorComoString(ValueWrapper valor) {
    string valorStr = valor switch
    {
        IntValue i => i.Value.ToString(),
        FloatValue f => f.Value.ToString(),
        StringValue s => "\"" + ProcesarEscapes(s.Value) + "\"", 
        BoolValue b => b.Value.ToString(),
        RuneValue r => "'" + r.Value.ToString() + "'",
        SliceValue slice => "[" + string.Join(", ", slice.Values.Select(v => ObtenerValorComoString(v))) + "]",
        _ => valor.ToString()
    };
    return valorStr;
}

private string ProcesarEscapes(string texto) {
    return texto
        .Replace("\\n", "\n")
        .Replace("\\t", "\t")
        .Replace("\\r", "\r")
        .Replace("\\\"", "\"")
        .Replace("\\\\", "\\");
}

//visitid
public override ValueWrapper VisitIdexpresion(LanguageParser.IdexpresionContext context)
{
    string id = context.ID().GetText();
    return currentEnvironment.GetVariable(id, context.Start);
}


//parens
    public override ValueWrapper VisitParens(LanguageParser.ParensContext context)
    {
      Console.WriteLine("Parens");
      return Visit(context.expr());
    }


//Negate
    public override ValueWrapper VisitNegate( LanguageParser.NegateContext context) {
    ValueWrapper valor = Visit(context.expr());

    return valor switch {
        IntValue i => new IntValue(-i.Value),
        FloatValue f => new FloatValue(-f.Value),
        _ => throw new SemanticError("Operación de negación no válida", context.Start)
    };
}

    
//INT
    public override ValueWrapper VisitInt(LanguageParser.IntContext context)
    {
      return new IntValue(int.Parse(context.INT().GetText()));
    }

//mulDiv
    public override ValueWrapper VisitMulDiv(LanguageParser.MulDivContext context)
    {
        ValueWrapper left = Visit(context.expr(0));
        ValueWrapper right = Visit(context.expr(1));
        var op = context.op.Text;

        return(left, right, op) switch {
            (IntValue l, IntValue r, "*") => new IntValue(l.Value * r.Value),
            (IntValue l, IntValue r, "/") => new IntValue(l.Value / r.Value),
            (IntValue l, FloatValue r, "*") => new FloatValue(l.Value * r.Value),
            (IntValue l, FloatValue r, "/") => new FloatValue(l.Value / r.Value),
            (FloatValue l, IntValue r, "*") => new FloatValue(l.Value * r.Value),
            (FloatValue l, IntValue r, "/") => new FloatValue(l.Value / r.Value),
            (FloatValue l, FloatValue r, "*") => new FloatValue(l.Value * r.Value),
            (FloatValue l, FloatValue r, "/") => new FloatValue(l.Value / r.Value),
            _ => throw new SemanticError("Operación de multiplicación o división no válida entre tipos: " + left.GetType() + " y " + right.GetType(), context.Start)
        };


    }

//addSub
public override ValueWrapper VisitAddSub(LanguageParser.AddSubContext context)
{
    ValueWrapper left = Visit(context.expr(0));
    ValueWrapper right = Visit(context.expr(1));
    var op = context.op.Text;

    return (left, right, op) switch {
        (IntValue l, IntValue r, "+") => new IntValue(l.Value * r.Value),
        (IntValue l, FloatValue r, "+") => new FloatValue(l.Value + r.Value),
        (FloatValue l, IntValue r, "+") => new FloatValue(l.Value + r.Value),
        (FloatValue l, FloatValue r, "+") => new FloatValue(l.Value + r.Value),
        (StringValue l, StringValue r, "+") => new StringValue(l.Value + r.Value),
        (IntValue l, IntValue r, "-") => new IntValue(l.Value - r.Value),
        (IntValue l, FloatValue r, "-") => new FloatValue(l.Value - r.Value),
        (FloatValue l, IntValue r, "-") => new FloatValue(l.Value - r.Value),
        (FloatValue l, FloatValue r, "-") => new FloatValue(l.Value - r.Value),
        _ => throw new SemanticError("Operación de suma o resta no válida entre tipos: " + left.GetType() + " y " + right.GetType(), context.Start)
    };
}
//Mod
    public override ValueWrapper VisitMod(LanguageParser.ModContext context)
    {
        ValueWrapper left = Visit(context.expr(0));
        ValueWrapper right = Visit(context.expr(1));
        var op = context.op.Text;

        return (left, right, op) switch {
            (IntValue l, IntValue r, "%") => new IntValue(l.Value % r.Value),
            _ => throw new SemanticError("Operación de módulo no válida entre tipos: " + left.GetType() + " y " + right.GetType(), context.Start)
        };
    }

//addSubMORE
    public override ValueWrapper VisitIncremento(LanguageParser.IncrementoContext context)
    {
        string nombreVariable = context.ID().GetText();
        string signo = context.GetChild(1).GetText();
        ValueWrapper valor = Visit(context.expr());
            
        if (signo == "+=") { // Incremento
            if (valor is IntValue){
                try {
                    var valorAntiguo = currentEnvironment.GetVariable(nombreVariable, context.Start);
                    Console.WriteLine("Variable " + nombreVariable + " valor antiguo: " + valorAntiguo + " nuevo valor: " + valor);
                    currentEnvironment.AssignVariable(nombreVariable, new IntValue(((IntValue)valorAntiguo).Value + ((IntValue)valor).Value), context.Start);
                } catch (Exception) {
                    Console.WriteLine("Error: La variable " + nombreVariable + " no es del mismo tipo.");
                    return new BoolValue(false);
                }
                }
            else if (valor is FloatValue){
                try {
                    var valorAntiguo = currentEnvironment.GetVariable(nombreVariable, context.Start);
                    Console.WriteLine("Variable " + nombreVariable + " valor antiguo: " + valorAntiguo + " nuevo valor: " + valor);
                    currentEnvironment.AssignVariable(nombreVariable, new FloatValue(((FloatValue)valorAntiguo).Value + ((FloatValue)valor).Value), context.Start);
                } catch (Exception) {
                    Console.WriteLine("Error: La variable " + nombreVariable + " no es del mismo tipo.");
                    return new BoolValue(false);
                }
            }else if (valor is StringValue){
                try {
                    var valorAntiguo = currentEnvironment.GetVariable(nombreVariable, context.Start);
                    Console.WriteLine("Variable " + nombreVariable + " valor antiguo: " + valorAntiguo + " nuevo valor: " + valor);
                    currentEnvironment.AssignVariable(nombreVariable, new StringValue(((StringValue)valorAntiguo).Value + ((StringValue)valor).Value), context.Start);
                } catch (Exception) {
                    Console.WriteLine("Error: La variable " + nombreVariable + " no es del mismo tipo.");
                    return new BoolValue(false);
                }
            }
            
        }
        if (valor is IntValue){
            if (signo == "-=") {
                try {
                    var valorAntiguo = currentEnvironment.GetVariable(nombreVariable, context.Start);
                    Console.WriteLine("Variable " + nombreVariable + " valor antiguo: " + valorAntiguo + " nuevo valor: " + valor);
                    currentEnvironment.AssignVariable(nombreVariable, new IntValue(((IntValue)valorAntiguo).Value - ((IntValue)valor).Value), context.Start);
                } catch (Exception) {
                    Console.WriteLine("Error: La variable " + nombreVariable + " no es del mismo tipo.");
                    return new BoolValue(false);
                }
            }
        } else if (valor is FloatValue){
            if (signo == "-=") { 
                try {
                    var valorAntiguo = currentEnvironment.GetVariable(nombreVariable, context.Start);
                    Console.WriteLine("Variable " + nombreVariable + " valor antiguo: " + valorAntiguo + " nuevo valor: " + valor);
                    currentEnvironment.AssignVariable(nombreVariable, new FloatValue(((FloatValue)valorAntiguo).Value - ((FloatValue)valor).Value), context.Start);
                } catch (Exception) {
                    Console.WriteLine("Error: La variable " + nombreVariable + " no es del mismo tipo.");
                    return new BoolValue(false);
                }
            }
        } 
        return new BoolValue(true);
    }

    /* VARIABLES */

public override ValueWrapper VisitDeclaracionVar(LanguageParser.DeclaracionVarContext context)
{
    string id = context.ID().GetText();
    string tipoStr = context.tipo()?.GetText() ?? "Desconocido";
    Type tipoEsperado = ObtenerTipo(tipoStr, context.Start);
    ValueWrapper valor = (context.expr() != null) ? Visit(context.expr()) : currentEnvironment.GetDefaultValue(tipoEsperado);

    
    if (context.Start == null) {
        throw new SemanticError($"Error interno: No se pudo obtener `Start` en la declaración de la variable '{id}'.", context.Start);
    }

    currentEnvironment.DeclareVariable(id, valor, tipoEsperado, context.Start);

    Console.WriteLine($" Variable '{id}' procesada en línea {context.Start.Line}, columna {context.Start.Column}.");

    return new BoolValue(true);
}

//declaracionImplicita
public override ValueWrapper VisitDeclaracionImplicita(LanguageParser.DeclaracionImplicitaContext context) {
    string id = context.ID().GetText();
    ValueWrapper valor = Visit(context.expr());

    Type tipoInferido = valor.GetType();

    if (context.Start == null) {
        throw new SemanticError($"Error interno: No se pudo obtener `Start` en la declaración implícita de la variable '{id}'.", context.Start);
    }

    currentEnvironment.DeclareVariable(id, valor, tipoInferido, context.Start);
    Console.WriteLine($" Variable '{id}' procesada en línea {context.Start.Line}, columna {context.Start.Column}.");

    return new BoolValue(true);
}
public Type ObtenerTipo(string tipo, Antlr4.Runtime.IToken? token) {
    return tipo switch {
        "int" => typeof(IntValue),
        "float64" => typeof(FloatValue),
        "bool" => typeof(BoolValue),
        "string" => typeof(StringValue),
        "rune" => typeof(RuneValue),
        _ when tipo.StartsWith("[]") => typeof(SliceValue),
        _ => ResolverTipoPersonalizado(tipo, token)
    };
}


public Type ResolverTipoPersonalizado(string tipo, Antlr4.Runtime.IToken? token)
{
    ValueWrapper tipoValor;

    try {
        tipoValor = currentEnvironment.GetVariable(tipo, token);
    } catch {
        throw new SemanticError("Error: Tipo no soportado o no declarado: " + tipo, token);
    }

    if (tipoValor is StructTypeValue) {
        return typeof(InstanceValue); 
    }

    throw new SemanticError("Error: El identificador '" + tipo + "' no es un tipo válido.", token);
}

    // asignaion
public override ValueWrapper VisitAsignarVar(LanguageParser.AsignarVarContext context) {
    var asignee = context.expr(0);
    ValueWrapper nuevoValor = Visit(context.expr(1));

    if (asignee is LanguageParser.IdexpresionContext idContext) {
        string id = idContext.ID().GetText();
        ValueWrapper valorAntiguo = currentEnvironment.GetVariable(id, context.Start);
        if (valorAntiguo.GetType() != nuevoValor.GetType()) {
            throw new SemanticError($"Error: No se puede cambiar el tipo de la variable {id}", context.Start);
        }
        currentEnvironment.AssignVariable(id, nuevoValor, context.Start);
        return defaultValue;
    } else if (asignee is LanguageParser.CalleeContext calleContext) {
        ValueWrapper callee = Visit(calleContext.expr());
        for (int i = 0; i < calleContext.call().Length; i++) {
            var action = calleContext.call(i);
            if (i == calleContext.call().Length - 1) {
                if (action is LanguageParser.GetContext propertyAcces) {
                    if (callee is InstanceValue instanceValue) {
                        var instance = instanceValue.Instance;
                        var propertyName = propertyAcces.ID().GetText();
                        instance.Set(propertyName, nuevoValor);
                    } else {
                        throw new SemanticError("Error: No se puede acceder a una propiedad de un valor que no es una instancia.", context.Start);
                    }
                } else {
                    throw new SemanticError("Error: No se puede asignar a una llamada que no es una propiedad.", context.Start);
                }
            }
            if (action is LanguageParser.FuncCallContext funcCall) {
                if (callee is FunctionValue functionValue) {
                    callee = VisitCall(functionValue.invocable, funcCall.args());
                } else {
                    throw new SemanticError("Error: No se puede llamar a un valor que no es una función.", context.Start);
                }
            } else if (action is LanguageParser.GetContext propertyAcces) {
                if (callee is InstanceValue instanceValue) {
                    callee = instanceValue.Instance.Get(propertyAcces.ID().GetText(), propertyAcces.Start, this);
                } else {
                    throw new SemanticError("Error: No se puede acceder a una propiedad de un valor que no es una instancia.", context.Start);
                }
            }
        }
        return callee;
    } else {
        throw new SemanticError("Error: No se puede asignar a un valor que no es una variable o una llamada.", context.Start);
    }
    return defaultValue;
}
public override ValueWrapper VisitAsignarSliceCompleto(LanguageParser.AsignarSliceCompletoContext context) {
    string id = context.ID().GetText();
    Type tipoEsperado = ObtenerTipo(context.tipo().GetText(), context.Start);
    List<ValueWrapper> valores = new List<ValueWrapper>();

    foreach (var exprCtx in context.expr()) {
        valores.Add(Visit(exprCtx));
    }

    SliceValue nuevoSlice = new SliceValue(valores);

    currentEnvironment.AssignVariable(id, nuevoSlice, context.Start);

    return new BoolValue(true);
}

//expresiones

    public override ValueWrapper VisitFloat( LanguageParser.FloatContext context) {
        return new FloatValue(float.Parse(context.FLOAT().GetText()));
    }


    public override ValueWrapper VisitBoleanTrueExpresion(LanguageParser.BoleanTrueExpresionContext context) {
        return new BoolValue(bool.Parse(context.TRUE().GetText()));
    }

    public override ValueWrapper VisitBoleanFalseExpresion(LanguageParser.BoleanFalseExpresionContext context) {
        return new BoolValue(bool.Parse(context.FALSE().GetText()));
    }

    public override ValueWrapper VisitCadenaExpresion( LanguageParser.CadenaExpresionContext context) {
        string cadena = context.GetText();
        if (cadena.StartsWith("\"") && cadena.EndsWith("\"")) {
            cadena = cadena.Substring(1, cadena.Length - 2);
        }
        return new StringValue(cadena);
    }

//caracter
    public override ValueWrapper VisitCaracterExpresion(LanguageParser.CaracterExpresionContext context) {
        string texto = context.GetText(); 
        if (texto.Length != 3 || texto[0] != '\'' || texto[2] != '\'') {
            throw new SemanticError("Error: Caracter inválido " + texto, context.Start);
        }

        char caracter = texto[1]; 
        return new RuneValue(caracter);
    }


    public override ValueWrapper VisitOperadorNegacion( LanguageParser.OperadorNegacionContext context) {
        ValueWrapper valor = Visit(context.expr());
        
        return valor switch {
            BoolValue b => new BoolValue(!b.Value),  // Negación lógica
            IntValue i => new IntValue(-i.Value),    // Negación aritmética
            FloatValue f => new FloatValue(-f.Value),    
            _ => throw new SemanticError("Operación de negación no válida", context.Start)
        };
    }

//relacionales
    public override ValueWrapper VisitRelational( LanguageParser.RelationalContext context) {

    ValueWrapper left = Visit(context.expr(0));
    ValueWrapper right = Visit(context.expr(1));
    var op = context.op.Text;

    return (left, right, op) switch {
        (IntValue l, IntValue r, "<") => new BoolValue(l.Value < r.Value),
        (IntValue l, IntValue r, "<=") => new BoolValue(l.Value <= r.Value),
        (IntValue l, IntValue r, ">") => new BoolValue(l.Value > r.Value),
        (IntValue l, IntValue r, ">=") => new BoolValue(l.Value >= r.Value),
        (FloatValue l, FloatValue r, "<") => new BoolValue(l.Value < r.Value),
        (FloatValue l, FloatValue r, "<=") => new BoolValue(l.Value <= r.Value),
        (FloatValue l, FloatValue r, ">") => new BoolValue(l.Value > r.Value),
        (FloatValue l, FloatValue r, ">=") => new BoolValue(l.Value >= r.Value),
        (IntValue l, FloatValue r, "<") => new BoolValue(l.Value < r.Value),
        (IntValue l, FloatValue r, "<=") => new BoolValue(l.Value <= r.Value),
        (IntValue l, FloatValue r, ">") => new BoolValue(l.Value > r.Value),
        (IntValue l, FloatValue r, ">=") => new BoolValue(l.Value >= r.Value),
        (FloatValue l, IntValue r, "<") => new BoolValue(l.Value < r.Value),
        (FloatValue l, IntValue r, "<=") => new BoolValue(l.Value <= r.Value),
        (FloatValue l, IntValue r, ">") => new BoolValue(l.Value > r.Value),
        (FloatValue l, IntValue r, ">=") => new BoolValue(l.Value >= r.Value),
        (RuneValue l, RuneValue r, "<") => new BoolValue(l.Value < r.Value),
        (RuneValue l, RuneValue r, "<=") => new BoolValue(l.Value <= r.Value),
        (RuneValue l, RuneValue r, ">") => new BoolValue(l.Value > r.Value),
        (RuneValue l, RuneValue r, ">=") => new BoolValue(l.Value >= r.Value),
        _ => throw new SemanticError("Operación relacional no válida entre tipos: " + left.GetType() + " y " + right.GetType(), context.Start)

        };
    }

// equalitys 
public override ValueWrapper VisitEqualitys(LanguageParser.EqualitysContext context) {
    ValueWrapper left = Visit(context.expr(0));
    ValueWrapper right = Visit(context.expr(1));
    var op = context.op.Text;

    if (left is SliceValue sliceLeft && right is SliceValue sliceRight) {
        bool resultado = sliceLeft.Values.SequenceEqual(sliceRight.Values);
        return new BoolValue(op == "==" ? resultado : !resultado);
    }
    if (left is SliceValue sliceLeftSingle && right is IntValue intRight) {
        bool resultado = sliceLeftSingle.Values.Count == 1 && sliceLeftSingle.Values[0] is IntValue singleValue && singleValue.Value == intRight.Value;
        return new BoolValue(op == "==" ? resultado : !resultado);
    }
    if (right is SliceValue sliceRightSingle && left is IntValue intLeft) {
        bool resultado = sliceRightSingle.Values.Count == 1 && sliceRightSingle.Values[0] is IntValue singleValue && singleValue.Value == intLeft.Value;
        return new BoolValue(op == "==" ? resultado : !resultado);
    }

    if (left is NullValue || right is NullValue)
    {
        bool isEqual = left is NullValue && right is NullValue;
        return new BoolValue(op == "==" ? isEqual : !isEqual);
    }

    return (left, right, op) switch {
        (IntValue l, IntValue r, "==") => new BoolValue(l.Value == r.Value),
        (IntValue l, IntValue r, "!=") => new BoolValue(l.Value != r.Value),
        (FloatValue l, FloatValue r, "==") => new BoolValue(l.Value == r.Value),
        (FloatValue l, FloatValue r, "!=") => new BoolValue(l.Value != r.Value),
        (StringValue l, StringValue r, "==") => new BoolValue(l.Value == r.Value),
        (StringValue l, StringValue r, "!=") => new BoolValue(l.Value != r.Value),
        (RuneValue l, RuneValue r, "==") => new BoolValue(l.Value == r.Value),
        (RuneValue l, RuneValue r, "!=") => new BoolValue(l.Value != r.Value),
        (BoolValue l, BoolValue r, "==") => new BoolValue(l.Value == r.Value),
        (BoolValue l, BoolValue r, "!=") => new BoolValue(l.Value != r.Value),
        _ => throw new SemanticError($"Error en comparación: {left.GetType()} {op} {right.GetType()}", context.Start)
    };
}


//incrementod

public override ValueWrapper VisitIncrementoDecremento(LanguageParser.IncrementoDecrementoContext context) {
    string variable = context.ID().GetText();
    string operador = context.GetChild(1).GetText();

    ValueWrapper valorActual = currentEnvironment.GetVariable(variable, context.Start);
    
    if (valorActual is not IntValue intValue) {
        throw new SemanticError($"Error: La variable {variable} no es un entero y no puede ser incrementada o decrementada.", context.Start);
    }

    int nuevoValor = operador == "++" ? intValue.Value + 1 : intValue.Value - 1;
    
    // Actualizar el valor en el entorno de variables
    currentEnvironment.AssignVariable(variable, new IntValue(nuevoValor), context.Start);

    return new IntValue(nuevoValor);
}



// logicos 
public override ValueWrapper VisitLogicos(LanguageParser.LogicosContext context) {
    ValueWrapper left = Visit(context.expr(0));
    ValueWrapper right = Visit(context.expr(1));
    var op = context.op.Text;

    if (left is SliceValue sliceLeft) {
        left = new BoolValue(sliceLeft.Values.Count > 0);
    }
    if (right is SliceValue sliceRight) {
        right = new BoolValue(sliceRight.Values.Count > 0);
    }

    if (left is not BoolValue || right is not BoolValue) {
        throw new SemanticError($"Operación lógica no válida entre tipos: {left.GetType()} y {right.GetType()}", context.Start);
    }

    return (left, right, op) switch {
        (BoolValue l, BoolValue r, "&&") => new BoolValue(l.Value && r.Value),
        (BoolValue l, BoolValue r, "||") => new BoolValue(l.Value || r.Value),
        _ => throw new SemanticError("Operación lógica no válida entre tipos: " + left.GetType() + " y " + right.GetType(), context.Start)
    };
}





//Visitblock
public override ValueWrapper VisitBloqueInstrucciones(LanguageParser.BloqueInstruccionesContext context) {
    Environment previousEnvironment = currentEnvironment;
    currentEnvironment = new Environment(previousEnvironment);

    foreach (var instruccion in context.listainstrucciones()) {
        ValueWrapper resultado = Visit(instruccion);

        if (resultado is BreakSignal || resultado is ContinueSignal) {
            currentEnvironment = previousEnvironment;
            return resultado; 
        }
    }

    currentEnvironment = previousEnvironment;
    return defaultValue;
}


//if 
public override ValueWrapper VisitIfInstruccion(LanguageParser.IfInstruccionContext context) {
    ValueWrapper condicion = Visit(context.expr());

    if (condicion is not BoolValue) {
        throw new SemanticError("Error: La condición del if no es booleana", context.Start);
    }

    if (((BoolValue)condicion).Value) {
        ValueWrapper resultado = Visit(context.instruccion(0));

        if (resultado is BreakSignal || resultado is ContinueSignal) return resultado;
    } 
    else if (context.instruccion().Length > 1) {
        ValueWrapper resultado = Visit(context.instruccion(1));

        if (resultado is BreakSignal || resultado is ContinueSignal) return resultado;
    }

    return defaultValue;
}

//while 
    public override ValueWrapper VisitWhileInstruccion(LanguageParser.WhileInstruccionContext context)
    {
        ValueWrapper condicion = Visit(context.expr());
        if (condicion is not BoolValue) {
            throw new SemanticError("Error: La condición del while no es booleana", context.Start);
        }

        while ((condicion as BoolValue).Value) {
            Visit(context.instruccion());
            condicion = Visit(context.expr());
            // if (condicion is not BoolValue) {
            //     throw new SemanticError("Error: La condición del while no es booleana");
            // }
        }

        return defaultValue;
    }

//switch
public override ValueWrapper VisitSwitchInstruccion(LanguageParser.SwitchInstruccionContext context) {
    ValueWrapper valorSwitch = Visit(context.expr());

    foreach (var caseCtx in context.cases()) {
        ValueWrapper caseValor = Visit(caseCtx.expr());

        if (valorSwitch.Equals(caseValor)) {
            foreach (var instruccion in caseCtx.listainstrucciones()) {
                ValueWrapper resultado = Visit(instruccion);

                if (resultado is BreakSignal) return resultado;
            }
            break;
        }
    }

    if (context.defaultCase() != null) {
        foreach (var instruccion in context.defaultCase().listainstrucciones()) {
            ValueWrapper resultado = Visit(instruccion);

            if (resultado is BreakSignal) return resultado;
        }
    }

    return new BoolValue(true);
}

    //declaracionSlice
public override ValueWrapper VisitDeclaracionSlice(LanguageParser.DeclaracionSliceContext context) {
    string id = context.ID().GetText();
    string tipoStr = context.tipo().GetText(); 
    Type tipoEsperado = ObtenerTipo(tipoStr, context.Start); 

    List<ValueWrapper> elementos = new List<ValueWrapper>();

    foreach (var exprCtx in context.expr()) {
        ValueWrapper valor = Visit(exprCtx);
        
        if (valor.GetType() != tipoEsperado) {
            throw new SemanticError("Error: El elemento " +valor +" no coincide con el tipo " + tipoStr, context.Start);
        }

        elementos.Add(valor);
    }

    currentEnvironment.DeclareVariable(id, new SliceValue(elementos), tipoEsperado, context.Start);

    Console.WriteLine("Declarado slice " +id+ "con valores:" + string.Join(", ", elementos));
    return new BoolValue(true);
}

public override ValueWrapper VisitDeclaracionSlicevacio(LanguageParser.DeclaracionSlicevacioContext context) {
    string id = context.ID().GetText();
    string tipoStr = context.tipo().GetText(); 
    Type tipoEsperado = ObtenerTipo(tipoStr, context.Start); 

    currentEnvironment.DeclareVariable(id, new SliceValue(new List<ValueWrapper>()), tipoEsperado, context.Start);

    Console.WriteLine("Declarado slice vacío: " + id + " de tipo " + tipoStr);
    return new BoolValue(true);
}

public override ValueWrapper VisitNilExpresion(LanguageParser.NilExpresionContext context)
{
    return new NullValue();
}


public override ValueWrapper VisitDeclaracionSlicemulti(LanguageParser.DeclaracionSlicemultiContext context) {
    string id = context.ID().GetText();
    string tipoStr = context.tipo().GetText();
    Type tipoEsperado = ObtenerTipo(tipoStr, context.Start);

    List<SliceValue> filas = new List<SliceValue>();

    foreach (var filaCtx in context.fila()) {
        List<ValueWrapper> filaValores = new List<ValueWrapper>();

        var subExpresiones = filaCtx.expr();

        foreach (var exprCtx in subExpresiones) {
            ValueWrapper valor = Visit(exprCtx);

            if (valor.GetType() != tipoEsperado) {
                throw new SemanticError($"Error: Todos los elementos de {id} deben ser de tipo {tipoEsperado}. Encontrado: {valor.GetType()}", context.Start);
            }

            filaValores.Add(valor);
        }

        if (filaValores.Count == 0) {
        }

        SliceValue filaSlice = new SliceValue(filaValores);
        filas.Add(filaSlice);
    }

    if (filas.Count == 0 || filas.All(f => f.Values.Count == 0)) {
    }

    SliceValue matriz = new SliceValue(filas.Cast<ValueWrapper>().ToList());
    currentEnvironment.DeclareVariable(id, matriz, tipoEsperado, context.Start);

    return new BoolValue(true);
}

//accesoslice
public override ValueWrapper VisitAccesoSlice(LanguageParser.AccesoSliceContext context) {
    ValueWrapper valor = Visit(context.expr(0));

    for (int i = 1; i < context.expr().Length; i++) {
        if (valor is not SliceValue slice) {
            throw new SemanticError("Error: No se puede indexar un valor que no es un slice.", context.Start);
        }

        ValueWrapper indiceValor = Visit(context.expr(i));
        if (indiceValor is not IntValue intIndice) {
            throw new SemanticError("Error: El índice debe ser un entero.", context.Start);
        }

        int indice = intIndice.Value;
        if (indice < 0 || indice >= slice.Values.Count) {
            throw new SemanticError($"Error: Índice fuera de rango en {indice}.", context.Start);
        }

        valor = slice.Values[indice];
    }

    return valor;
}

public override ValueWrapper VisitAsignarSlice(LanguageParser.AsignarSliceContext context) {
    string id = context.ID().GetText();
    ValueWrapper sliceValor = currentEnvironment.GetVariable(id, context.Start);

    if (sliceValor is not SliceValue slice) {
        throw new SemanticError($"Error: {id} no es un slice y no puede indexarse.", context.Start);
    }

    ValueWrapper indiceValor = Visit(context.expr(0));
    if (indiceValor is not IntValue intIndice) {
        throw new SemanticError("Error: El índice debe ser un entero.", context.Start);
    }

    int indice = intIndice.Value;
    if (indice < 0 || indice >= slice.Values.Count) {
        throw new SemanticError($"Error: Índice fuera de rango en {id}[{indice}].", context.Start);
    }

    ValueWrapper nuevoValor = Visit(context.expr(1));

    // Reemplazar el valor en la posición indicada
    slice.Values[indice] = nuevoValor;
    Console.WriteLine($"Asignado valor {ObtenerValorComoString(nuevoValor)} en {id}[{indice}]");

    return new BoolValue(true);
}
public override ValueWrapper VisitAsignarSliceMulti(LanguageParser.AsignarSliceMultiContext context) {
    string id = context.ID().GetText();
    ValueWrapper sliceValor = currentEnvironment.GetVariable(id, context.Start);

    if (sliceValor is not SliceValue slice) {
        throw new SemanticError($"Error: {id} no es un slice multidimensional y no puede indexarse.", context.Start);
    }

    ValueWrapper indiceFilaValor = Visit(context.expr(0));
    if (indiceFilaValor is not IntValue intFila) {
        throw new SemanticError("Error: El índice de fila debe ser un entero.", context.Start);
    }

    int fila = intFila.Value;
    if (fila < 0 || fila >= slice.Values.Count) {
        throw new SemanticError($"Error: Índice fuera de rango en {id}[{fila}].", context.Start);
    }

    ValueWrapper filaSlice = slice.Values[fila];

    if (filaSlice is not SliceValue subSlice) {
        throw new SemanticError($"Error: {id}[{fila}] no es un slice válido.", context.Start);
    }

    ValueWrapper indiceColumnaValor = Visit(context.expr(1));
    if (indiceColumnaValor is not IntValue intColumna) {
        throw new SemanticError("Error: El índice de columna debe ser un entero.", context.Start);
    }

    int columna = intColumna.Value;
    if (columna < 0 || columna >= subSlice.Values.Count) {
        throw new SemanticError($"Error: Índice fuera de rango en {id}[{fila}][{columna}].", context.Start);
    }

    ValueWrapper nuevoValor = Visit(context.expr(2));

    subSlice.Values[columna] = nuevoValor;

    return new BoolValue(true);
}

public override ValueWrapper VisitAccesoSliceMulti(LanguageParser.AccesoSliceMultiContext context) {
    ValueWrapper valor = Visit(context.expr(0)); // Obtener la matriz principal


    if (valor is not SliceValue slice) {
        throw new SemanticError($"Error: No se puede indexar un valor que no es un slice. Se obtuvo {valor.GetType()} en {context.expr(0).GetText()}.", context.Start);
    }

    // Obtener el índice de la primera dimensión
    ValueWrapper indiceFilaValor = Visit(context.expr(1));
    if (indiceFilaValor is not IntValue intFila) {
        throw new SemanticError("Error: El índice de fila debe ser un entero.", context.Start);
    }

    int fila = intFila.Value;
    if (fila < 0 || fila >= slice.Values.Count) {
        throw new SemanticError($"Error: Índice fuera de rango en [{fila}].", context.Start);
    }

    ValueWrapper filaSlice = slice.Values[fila];


    if (filaSlice is not SliceValue subSlice) {
        throw new SemanticError($"Error: {context.expr(0).GetText()}[{fila}] no es un slice válido. Se obtuvo {filaSlice.GetType()}", context.Start);
    }

    // Obtener el índice de la segunda dimensión
    ValueWrapper indiceColumnaValor = Visit(context.expr(2));
    if (indiceColumnaValor is not IntValue intColumna) {
        throw new SemanticError("Error: El índice de columna debe ser un entero.", context.Start);
    }

    int columna = intColumna.Value;
    if (columna < 0 || columna >= subSlice.Values.Count) {
        throw new SemanticError($"Error: Índice fuera de rango en [{fila}][{columna}].", context.Start);
    }

    ValueWrapper resultado = subSlice.Values[columna];

    if (resultado is SliceValue) {
        throw new SemanticError($"Error: Se esperaba un valor escalar, pero se obtuvo un SliceValue en {context.expr(0).GetText()}[{fila}][{columna}].", context.Start);
    }


    return resultado;
}

//forcondicion
public override ValueWrapper VisitForCondicion(LanguageParser.ForCondicionContext context) {
    Environment previousEnvironment = currentEnvironment;
    currentEnvironment = new Environment(previousEnvironment);

    while (true) {
        ValueWrapper condicion = Visit(context.expr());

        if (condicion is BoolValue b) {
            if (!b.Value) break; 
        } else {
            throw new SemanticError("Error: La condición del for debe ser un booleano.", context.Start);
        }

        ValueWrapper resultado = Visit(context.instruccion());

        if (resultado is BreakSignal) break;

        if (resultado is ContinueSignal) {
            continue;
        }
    }

    currentEnvironment = previousEnvironment;

    return new BoolValue(true);
}


//forinicializacion
public override ValueWrapper VisitForincicializacion(LanguageParser.ForincicializacionContext context) {
    Environment previousEnvironment = currentEnvironment;
    currentEnvironment = new Environment(previousEnvironment);

    if (context.asignacion() != null || context.variables() != null) {
        if (context.asignacion() != null) {
            Visit(context.asignacion());
        } else if (context.variables() != null) {
            Visit(context.variables());
        }
    }

    while (true) {
        if (context.expr(0) != null) {
            ValueWrapper condicion = Visit(context.expr(0));
            if (condicion is BoolValue b && !b.Value) break;
        }

        ValueWrapper resultado = Visit(context.instruccion());

        if (resultado is BreakSignal) break;

        if (resultado is ContinueSignal) {
            if (context.expr(1) != null) {
                Visit(context.expr(1)); 
            }
            continue;
        }

        if (context.expr(1) != null) {
            Visit(context.expr(1));
        }
    }

    currentEnvironment = previousEnvironment;
    return new BoolValue(true);
}


//forrange
public override ValueWrapper VisitForRange(LanguageParser.ForRangeContext context) {
    string indexVar = context.ID(0).GetText();
    string valueVar = context.ID(1).GetText();
    string sliceName = context.ID(2).GetText();

    // Obtener el slice del entorno
    ValueWrapper sliceValue = currentEnvironment.GetVariable(sliceName, context.Start);
    if (sliceValue is not SliceValue slice) {
        throw new SemanticError($"Error: {sliceName} no es un slice.", context.Start);
    }

    // Guardar el entorno actual y crear un nuevo scope para el for
    Environment previousEnvironment = currentEnvironment;
    currentEnvironment = new Environment(previousEnvironment);

    for (int i = 0; i < slice.Values.Count; i++) {
        ValueWrapper indexValue = new IntValue(i);
        ValueWrapper elementValue = slice.Values[i];

        if (currentEnvironment.variables.ContainsKey(indexVar)) {
            currentEnvironment.AssignVariable(indexVar, indexValue, context.Start);
        } else {
            currentEnvironment.DeclareVariable(indexVar, indexValue, typeof(IntValue));
        }

        if (currentEnvironment.variables.ContainsKey(valueVar)) {
            currentEnvironment.AssignVariable(valueVar, elementValue, context.Start);
        } else {
            currentEnvironment.DeclareVariable(valueVar, elementValue, elementValue.GetType());
        }

        ValueWrapper resultado = Visit(context.instruccion());

        if (resultado is ContinueSignal) {
            continue;
        }
    }

    currentEnvironment = previousEnvironment;

    return new BoolValue(true);
}

//break

public override ValueWrapper VisitBreakInstruccion(LanguageParser.BreakInstruccionContext context) {
    return new BreakSignal();
}

//return
public override ValueWrapper VisitReturnInstruccion(LanguageParser.ReturnInstruccionContext context) {
    ValueWrapper valor = defaultValue; 

    if (context.expr() != null) {
        valor = Visit(context.expr()); 
    }

    throw new ReturnException(valor);
}

public override ValueWrapper VisitContinueInstruccion(LanguageParser.ContinueInstruccionContext context) {
    return new ContinueSignal();
}

//callee 
public override ValueWrapper VisitCallee(LanguageParser.CalleeContext context) {
    ValueWrapper callee = Visit(context.expr());


    if (callee is InstanceValue iv) {
    }

    foreach (var action in context.call()) {
        if (action is LanguageParser.FuncCallContext funcall) {

            if (callee is FunctionValue functionValue) {
                callee = VisitCall(functionValue.invocable, funcall.args());
            } else {
                throw new SemanticError("Error: No se puede llamar a un valor que no es una función.", context.Start);
            }
        }
        else if (action is LanguageParser.GetContext propertyAccess) {

            if (callee is InstanceValue instanceValue) {
                callee = instanceValue.Instance.Get(propertyAccess.ID().GetText(), propertyAccess.Start, this);
            } else {
                throw new SemanticError($"Error: No se puede acceder a la propiedad '{propertyAccess.ID().GetText()}' en algo que no es una instancia (tipo: {callee?.GetType().Name})",
                    propertyAccess.Start
                );
            }
        }
    }

    return callee;
}


public ValueWrapper VisitCall(Invocable invocable, LanguageParser.ArgsContext context) {
    List<ValueWrapper> argumentos = new List<ValueWrapper>();

    if(context != null) {
        foreach (var expr in context.expr()) {
            argumentos.Add(Visit(expr));
        }
    }

    if(context != null && argumentos.Count != invocable.Arity()) {
        throw new SemanticError("Error: Número de argumentos incorrecto.", context.Start);
    }

    return invocable.Invoke(argumentos, this);

}

//funcdlc
private static string ObtenerNombreFuncion(LanguageParser.FuncdlcContext context) {
    return context.ID().Length switch {
        1 => context.ID(0).GetText(),     // func normal
        3 => context.ID(2).GetText(),     // método con receptor
        _ => throw new SemanticError("Error: No se puede determinar el nombre de la función.", context.Start)
    };
}

private static string? ObtenerTipoReceptor(LanguageParser.FuncdlcContext context) {
    return context.ID().Length == 3 ? context.ID(1).GetText() : null; // Receptor si es método
}

private Dictionary<string, List<(string name, ForeignFunction func)>> visitorMethods = new();


public override ValueWrapper VisitFuncdlc(LanguageParser.FuncdlcContext context) {
    string nombreFuncion = ObtenerNombreFuncion(context);
    string? tipoReceptor = ObtenerTipoReceptor(context); 

    Type tipoRetorno = typeof(VoidValue);
    string tipoStr = "void";

    if (context.tipo() != null) { 
        tipoRetorno = ObtenerTipo(context.tipo().GetText(), context.Start);
        tipoStr = context.tipo().GetText();
    }

    var foreign = new ForeignFunction(currentEnvironment, context, tipoRetorno, this);

    if (context.Start == null) {
        throw new SemanticError($"Error interno: No se pudo obtener 'Start' para la función '{nombreFuncion}'.", context.Start);
    }

    if (tipoReceptor == null) {
        if (currentEnvironment.variables.ContainsKey(nombreFuncion)) {
            throw new SemanticError("Error: La función '" + nombreFuncion + "' ya ha sido declarada.", context.Start);
        }

        currentEnvironment.DeclareVariable(nombreFuncion, new FunctionValue(foreign, nombreFuncion), typeof(FunctionValue), context.Start);

        int line = context.Start.Line;
        int column = context.Start.Column;
        InterpreterVisitor.GlobalSymbolTable.AddSymbol(nombreFuncion, "Function", tipoStr, "Global", line, column);

    } else {
        if (!visitorMethods.ContainsKey(tipoReceptor)) {
            visitorMethods[tipoReceptor] = new List<(string, ForeignFunction)>();
        }
        visitorMethods[tipoReceptor].Add((nombreFuncion, foreign));

    }

    return defaultValue;
}

//Visitstructdcl
public override ValueWrapper VisitStructdcl(LanguageParser.StructdclContext context) {
    Dictionary<string, StructFieldInfo> props = new();
    Dictionary<string, ForeignFunction> methods = new();

    string structName = context.ID().GetText();

    foreach (var prop in context.structBody()) {
        if (prop.variables() is LanguageParser.DeclaracionVarContext declaracionVar) {
            string id = declaracionVar.ID().GetText();
            string tipo = declaracionVar.tipo().GetText();
            props[id] = new StructFieldInfo(id, tipo);
        } 
        else if (prop.ID() != null && prop.tipo() != null) {
            string id = prop.ID().GetText();
            string tipo = prop.tipo().GetText();
            props[id] = new StructFieldInfo(id, tipo);
        } 
        else if (prop.funcdlc() != null) {
            // Visitar el método dentro del struct
            Visit(prop.funcdlc());

        
            if (visitorMethods.ContainsKey(structName)) {
                foreach (var (name, method) in visitorMethods[structName]) {
                    methods[name] = method;
                }

                visitorMethods.Remove(structName); 
            }
        }
    }

    //  métodos definidos fuera del struct
    if (visitorMethods.ContainsKey(structName)) {
        foreach (var (name, method) in visitorMethods[structName]) {
            methods[name] = method;
        }

        visitorMethods.Remove(structName); // Limpiar
    }

    StructType structType = new StructType(structName, props, methods);
    currentEnvironment.DeclareVariable(structName, new StructTypeValue(structType), typeof(StructTypeValue));

    return defaultValue;
}


//visitnew
public override ValueWrapper VisitNew(LanguageParser.NewContext context) {
    ValueWrapper  structTypeValue = currentEnvironment.GetVariable(context.ID().GetText(), context.Start);

    if (structTypeValue is not StructTypeValue) {
        throw new SemanticError("Error: No se puede instanciar un valor que no es una clase.", context.Start);
    }
    List<ValueWrapper> argumentos = new List<ValueWrapper>();

    if(context.args() != null) {
        foreach (var arg in context.args().expr()) {
            argumentos.Add(Visit(arg));
        }

    }
    var instance = ((StructTypeValue)structTypeValue).StructType.Invoke(argumentos, this);
    Console.WriteLine("Instancia de " + context.ID().GetText() + " creada.");
    return instance;
    
}

public override ValueWrapper VisitModuleFuncCall(LanguageParser.ModuleFuncCallContext context)
{
    var instance = Visit(context.expr());

    if (instance is InstanceValue instVal)
    {
        string methodName = context.ID().GetText();

        var method = instVal.Instance.Get(methodName, context.ID().Symbol, this);

        if (method is FunctionValue func)
        {
            var args = context.args()?.expr().Select(Visit).ToList() ?? new List<ValueWrapper>();
            return func.invocable.Invoke(args, this); 
        }

        throw new SemanticError($"'{methodName}' no es una función en la instancia.", context.ID().Symbol);
    }

    throw new SemanticError("Llamada de método sobre un valor que no es instancia.", context.Start);
}

public void EjecutarMain(InterpreterVisitor visitor, Antlr4.Runtime.ParserRuleContext context) {
    string mainFunctionName = "main";

    if (!currentEnvironment.variables.ContainsKey(mainFunctionName)) {
        throw new SemanticError("Error: No se encontró la función 'main'.", context.Start);
    }

    ValueWrapper functionWrapper = currentEnvironment.GetVariable(mainFunctionName, new Antlr4.Runtime.CommonToken(0));

    if (functionWrapper is not FunctionValue functionValue) {
        throw new SemanticError("Error: 'main' está declarada pero no es una función.", context.Start);
    }

    if (functionValue.invocable.Arity() != 0) {
        throw new SemanticError("Error: La función 'main' no debe recibir parámetros.", context.Start );
    }

    Console.WriteLine("Ejecutando función 'main'...");
    
    // Ejecutar main
    functionValue.invocable.Invoke(new List<ValueWrapper>(), visitor);
}


public override ValueWrapper VisitInstanciaStruct(LanguageParser.InstanciaStructContext context)
{
    string tipoNombre = context.ID().GetText();
    var structTypeValue = currentEnvironment.GetVariable(tipoNombre, context.Start) as StructTypeValue;

    if (structTypeValue == null)
    {
        throw new SemanticError($"Struct '{tipoNombre}' no está definido.", context.Start);
    }

    var instance = new Instance(structTypeValue.StructType);
    foreach (var campo in context.camposStruct().campoStruct())
    {
        string campoNombre = campo.ID().GetText();
        ValueWrapper valor = Visit(campo.expr());
        instance.Set(campoNombre, valor);
    }

    return new InstanceValue(instance);
}

}

