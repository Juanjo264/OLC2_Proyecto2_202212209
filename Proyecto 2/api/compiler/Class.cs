public class StructType : Invocable {
    public string Name { get; set; }
    public Dictionary<string, StructFieldInfo> Props { get; set; } 
    public Dictionary<string, ForeignFunction> Methods { get; set; }
    public string MainProperty { get; private set; } = "";

    public StructType(string name,
        Dictionary<string, StructFieldInfo> props,          
        Dictionary<string, ForeignFunction> methods) {
        
        Name = name;
        Props = props;
        Methods = methods;

        // Buscar propiedad principal
        foreach (var prop in Props) {
            var tipo = prop.Value.TypeName;
            if (tipo == "int" || tipo == "float64") {
                MainProperty = prop.Key;
                break;
            }
        }
    }

    public ForeignFunction? GetMethod(string name) {
        return Methods.ContainsKey(name) ? Methods[name] : null;
    }

    public int Arity() {
        var constructor = GetMethod("constructor");
        return constructor?.Arity() ?? 0;
    }

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor) {
        var instance = new Instance(this, MainProperty);

        foreach (var prop in Props) {
            var name = prop.Key;
            var fieldInfo = prop.Value;

            Type tipo = visitor.ObtenerTipo(fieldInfo.TypeName, null);  
            ValueWrapper valor = visitor.currentEnvironment.GetDefaultValue(tipo);
            instance.Set(name, valor);
        }

        var constructor = GetMethod("constructor");
        if (constructor != null) {
            constructor.Bind(instance, visitor).Invoke(args, visitor);
        }

        return new InstanceValue(instance);
    }
}

public class StructFieldInfo {
    public string Name { get; }
    public string TypeName { get; }

    public StructFieldInfo(string name, string typeName) {
        Name = name;
        TypeName = typeName;
    }
}

