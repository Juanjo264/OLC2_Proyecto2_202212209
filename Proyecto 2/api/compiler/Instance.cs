public class Instance {
    private StructType structTypeValue;
    private Dictionary<string, ValueWrapper> Properties;
    private string mainProperty; // Propiedad principal de la instancia

    public Instance(StructType structTypeValue, string mainProperty = "") {
        this.structTypeValue = structTypeValue;
        this.mainProperty = mainProperty;
        Properties = new Dictionary<string, ValueWrapper>();
    }

    public void Set(string name, ValueWrapper value) {
        Properties[name] = value;
    }

    public ValueWrapper Get(string name, Antlr4.Runtime.IToken token, CompilerVisitor visitor)
    {
        if (Properties.ContainsKey(name)) {
            return Properties[name];
        }

        var method = structTypeValue.GetMethod(name);
        if (method != null) {
            return new FunctionValue(method.Bind(this, visitor), name);
        }

        throw new SemanticError("Propiedad " + name + " no encontrada", token);
    }

    public ValueWrapper GetMainProperty() {
        if (!string.IsNullOrEmpty(mainProperty) && Properties.ContainsKey(mainProperty)) {
            return Properties[mainProperty]; // retorna la propiedad principal
        }
        return new InstanceValue(this); 
    }

    public StructType GetClass() {
        return structTypeValue;
    }

    public override string ToString() {
        var propsStr = string.Join(", ",
            Properties.Select(kv => $"{kv.Key}: {FormatValue(kv.Value)}"));
        return $"{{ {propsStr} }}";
    }

    private string FormatValue(ValueWrapper value) {
        return value switch {
            StringValue s => $"\"{s.Value}\"",
            IntValue i => i.Value.ToString(),
            FloatValue f => f.Value.ToString(),
            BoolValue b => b.Value.ToString().ToLower(),
            RuneValue r => $"'{r.Value}'",
            InstanceValue inst => inst.Instance.ToString(), // Recursivo
            SliceValue slice => slice.ToString(),
            NullValue => "nil",
            _ => value?.ToString() ?? "null"
        };
    }
}
