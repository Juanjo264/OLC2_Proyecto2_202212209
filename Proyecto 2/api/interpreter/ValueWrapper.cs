public abstract record ValueWrapper ;

public record IntValue(int Value) : ValueWrapper;

public record FloatValue(float Value) : ValueWrapper;

public record StringValue(string Value) : ValueWrapper;

public record BoolValue(bool Value) : ValueWrapper;
public record ContinueSignal : ValueWrapper { }
public record BreakSignal : ValueWrapper { }
public record NullValue : ValueWrapper;

public record InstanceValue(Instance Instance) : ValueWrapper;
public record RuneValue(char Value) : ValueWrapper;
public record VoidValue : ValueWrapper;
public record FunctionValue(Invocable invocable, string name) : ValueWrapper;

public record StructTypeValue(StructType StructType) : ValueWrapper;

public record SliceValue : ValueWrapper {
    public List<ValueWrapper> Values { get; set; } = new List<ValueWrapper>(); 

    public SliceValue(List<ValueWrapper> valores) {
        this.Values = valores ?? new List<ValueWrapper>(); 
    }

    public override string ToString() {
        if (Values.Count == 0) {
            return "[]"; 
        }

        return "[" + string.Join(", ", Values.Select(v => ObtenerValorComoString(v))) + "]";
    }

    public static string ObtenerValorComoString(ValueWrapper value) {
        return value switch {
            IntValue i => i.Value.ToString(),
            FloatValue f => f.Value.ToString(),
            StringValue s => $"\"{s.Value}\"",
            BoolValue b => b.Value.ToString().ToLower(),
            RuneValue r => r.Value.ToString(), 
            SliceValue slice => slice.ToString(), 
            _ => throw new SemanticError($"Tipo no soportado en slice: {value.GetType()}", new Antlr4.Runtime.CommonToken(0))
        };
    }
}

