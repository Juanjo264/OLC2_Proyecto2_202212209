using System.Linq.Expressions;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
public class ForeignFunction : Invocable {
    public Environment closure;
    public LanguageParser.FuncdlcContext context;
    public Type tipoRetorno;
    public List<Type> tiposParametros = new List<Type>();

private CompilerVisitor visitor; 

public ForeignFunction(Environment closure, LanguageParser.FuncdlcContext context, Type tipoRetorno, CompilerVisitor visitor)
{
    this.closure = closure;
    this.context = context;
    this.tipoRetorno = tipoRetorno;
    this.visitor = visitor;

    if (context.@params() != null) {
        foreach (var param in context.@params().param()) {
            tiposParametros.Add(visitor.ObtenerTipo(param.tipo().GetText(), param.tipo().Start));
        }
    }
}

    public int Arity() {
        return tiposParametros.Count;
    }

public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor) {
    if (args.Count != tiposParametros.Count) {
        throw new SemanticError($"Error: Se esperaban {tiposParametros.Count} argumentos, pero se recibieron {args.Count}.", context.Start);
    }

    var newEnv = new Environment(closure);
    var beforeCallEnv = visitor.currentEnvironment;
    visitor.currentEnvironment = newEnv;

    bool hasReturned = false; 

    if (context.@params() != null) {
        for (int i = 0; i < context.@params().param().Length; i++) {
            var paramName = context.@params().param(i).ID().GetText();
            var expectedType = tiposParametros[i];

            if (args[i].GetType() != expectedType) {
                throw new SemanticError($"Error: Se esperaba un argumento de tipo {expectedType.Name} para '{paramName}', pero se recibió {args[i].GetType().Name}.", context.Start);
            }

            newEnv.DeclareVariable(paramName, args[i]);
        }
    }

    try {
        foreach (var statement in context.listainstrucciones()) {
            visitor.Visit(statement);
        }
    } catch (ReturnException e) {
        visitor.currentEnvironment = beforeCallEnv;
        hasReturned = true;  

        if (tipoRetorno != typeof(VoidValue) && e.Value.GetType() != tipoRetorno) {
            throw new SemanticError($"Error: La función debe retornar {tipoRetorno.Name}, pero dio {e.Value.GetType().Name}.", context.Start);
        }

        return e.Value;
    }

visitor.currentEnvironment = beforeCallEnv;

string nombreFuncion;

if (context.ID().Length == 2) {
    // Método con receptor 
    nombreFuncion = context.ID(1).GetText();
} else {
    // Función normal
    nombreFuncion = context.ID(0).GetText();
}

if (!hasReturned && tipoRetorno != typeof(VoidValue)) {
    throw new SemanticError($"Error: La función {nombreFuncion} debe retornar {tipoRetorno.Name}, pero no tiene return", context.Start);
}

return visitor.defaultValue; 
}



public Invocable Bind(Instance instance, CompilerVisitor visitor)
{
    return new BoundForeignFunction(this, instance, visitor);
}




public static ValueWrapper ObtenerValorPorDefecto(Type tipo) {
    return tipo switch {
        Type t when t == typeof(IntValue) => new IntValue(0),
        Type t when t == typeof(FloatValue) => new FloatValue(0.0f),
        Type t when t == typeof(BoolValue) => new BoolValue(false),
        Type t when t == typeof(StringValue) => new StringValue(""),
        Type t when t == typeof(RuneValue) => new RuneValue('\0'),
        Type t when t == typeof(SliceValue) => new SliceValue(new List<ValueWrapper>()),
        Type t when t == typeof(InstanceValue) => new InstanceValue(null), 
        _ => throw new SemanticError($"Error: No se puede determinar un valor por defecto para el tipo {tipo.Name}.", null)
    };
}

}
