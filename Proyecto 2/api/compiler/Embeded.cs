public class Embeded
{
public static void Generate(Environment env)
{
    env.DeclareVariable("time", new FunctionValue(new TimeEmbeded(), "time"));
    env.DeclareVariable("reflect.TypeOf", new FunctionValue(new TypeOfEmbeded(), "reflect.TypeOf"));
    env.DeclareVariable("slices.Index", new FunctionValue(new IndexEmbeded(), "slices.Index"));
    env.DeclareVariable("strings.Join", new FunctionValue(new JoinEmbeded(), "strings.Join"));
    env.DeclareVariable("len", new FunctionValue(new LenEmbeded(), "len"));
    env.DeclareVariable("append", new FunctionValue(new AppendEmbeded(), "append"));

    // Crear struct para strconv
    var strconvFields = new Dictionary<string, ValueWrapper>
    {
        { "Atoi", new FunctionValue(new AtoiEmbeded(), "Atoi") },
        { "ParseFloat", new FunctionValue(new ParseFloatEmbeded(), "ParseFloat") }
    };

    var strconvInstance = new Instance(
        new StructType("strconv", new(), new()) // Struct vacío
    );
    // --------- reflect.TypeOf
    var reflectInstance = new Instance(
        new StructType("reflect", new(), new())
    );
    reflectInstance.Set("TypeOf", new FunctionValue(new TypeOfEmbeded(), "TypeOf"));
    env.DeclareVariable("reflect", new InstanceValue(reflectInstance), typeof(InstanceValue));
    // --------- slices.Index
    var slicesInstance = new Instance(
        new StructType("slices", new(), new())
    );
    slicesInstance.Set("Index", new FunctionValue(new IndexEmbeded(), "Index"));
    env.DeclareVariable("slices", new InstanceValue(slicesInstance), typeof(InstanceValue));

    // --------- strings.Join
    var stringsInstance = new Instance(
        new StructType("strings", new(), new())
    );
    stringsInstance.Set("Join", new FunctionValue(new JoinEmbeded(), "Join"));
    env.DeclareVariable("strings", new InstanceValue(stringsInstance), typeof(InstanceValue));

    foreach (var kv in strconvFields)
    {
        strconvInstance.Set(kv.Key, kv.Value); 
    }

   
    env.DeclareVariable("strconv", new InstanceValue(strconvInstance), typeof(InstanceValue));
}
}

public class TimeEmbeded : Invocable
{
    
    public int Arity(){
      return 0;
    }
    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor){
        return new StringValue(DateTime.Now.ToString());
    }
}

public class AtoiEmbeded : Invocable
{
    public int Arity() => 1; 

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        if (args[0] is not StringValue str)
        {
            throw new SemanticError("Error: strconv.Atoi requiere una cadena como argumento.", token: null);
        }

        if (!int.TryParse(str.Value, out int resultado))
        {
            throw new SemanticError($"Error: strconv.Atoi no puede convertir '{str.Value}' a un número entero.", token: null);
        }

        return new IntValue(resultado);
    }
}
public class ParseFloatEmbeded : Invocable
{
    public int Arity() => 1; 

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        if (args[0] is not StringValue str)
        {
            throw new SemanticError("Error: strconv.ParseFloat requiere una cadena como argumento.", token: null);
        }

        if (!double.TryParse(str.Value, out double resultado))
        {
            throw new SemanticError($"Error: strconv.ParseFloat no puede convertir '{str.Value}' a un número flotante.", token: null);
        }

        return new FloatValue((float)resultado); 
    }
}


public class TypeOfEmbeded : Invocable
{
    public int Arity() => 1; 

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        ValueWrapper valor = args[0];

        if (valor is SliceValue slice)
        {
            if (slice.Values.Count > 0)
            {
                string tipoElementos = ObtenerTipoComoString(slice.Values[0]);
                return new StringValue($"[]{tipoElementos}");
            }
            return new StringValue("[]unknown"); 
        }

        return new StringValue(ObtenerTipoComoString(valor));
    }

    private string ObtenerTipoComoString(ValueWrapper valor)
    {
        return valor switch
        {
            IntValue => "int",
            FloatValue => "float64",
            StringValue => "string",
            BoolValue => "bool",
            RuneValue => "rune",
            SliceValue => "slice", 
            _ => "unknown"
        };
    }
}


public class IndexEmbeded : Invocable
{
    public int Arity() => 2; // slice, valor a buscar

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        if (args[0] is not SliceValue slice)
        {
            throw new SemanticError("Error: slices.Index requiere un slice como primer argumento.", token: null);
        }

        ValueWrapper valorBuscar = args[1];

        for (int i = 0; i < slice.Values.Count; i++)
        {
            if (slice.Values[i].Equals(valorBuscar))
            {
                return new IntValue(i);
            }
        }

        return new IntValue(-1);
    }
}


public class JoinEmbeded : Invocable
{
    public int Arity() => 2; // slice de strings, separador

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        if (args[0] is not SliceValue slice)
        {
            throw new SemanticError("Error: strings.Join requiere un slice de strings como primer argumento.", token: null);
        }

        if (args[1] is not StringValue separador)
        {
            throw new SemanticError("Error: strings.Join requiere un string como separador.", token: null);
        }

        List<string> elementos = new List<string>();

        foreach (var val in slice.Values)
        {
            if (val is not StringValue strVal)
            {
                throw new SemanticError("Error: strings.Join solo puede usarse con slices de strings.", token:  null);
            }
            elementos.Add(strVal.Value);
        }

        return new StringValue(string.Join(separador.Value, elementos));
    }
}


public class LenEmbeded : Invocable
{
    public int Arity() => 1; // slice

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        if (args[0] is not SliceValue slice)
        {
            throw new SemanticError("Error: len requiere un slice como argumento.", token:  null);
        }

        return new IntValue(slice.Values.Count);
    }
}


public class AppendEmbeded : Invocable
{
    public int Arity() => 2; // slice, elementos a agregar

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        if (args[0] is not SliceValue slice)
        {
            throw new SemanticError("Error: append requiere un slice como primer argumento.", token:  null);
        }

        List<ValueWrapper> nuevosValores = new List<ValueWrapper>(slice.Values);
        nuevosValores.Add(args[1]); // Agregamos el nuevo valor al slice

        return new SliceValue(nuevosValores);
    }
}
