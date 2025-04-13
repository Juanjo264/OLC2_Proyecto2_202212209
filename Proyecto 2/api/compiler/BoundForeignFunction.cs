public class BoundForeignFunction : Invocable
{
    private readonly ForeignFunction funcionOriginal;
    private readonly Instance instancia;
    private readonly CompilerVisitor visitor;

    public BoundForeignFunction(ForeignFunction funcionOriginal, Instance instancia, CompilerVisitor visitor)
    {
        this.funcionOriginal = funcionOriginal;
        this.instancia = instancia;
        this.visitor = visitor;
    }

    public int Arity() => funcionOriginal.Arity();

    public ValueWrapper Invoke(List<ValueWrapper> args, CompilerVisitor visitor)
    {
        var newEnv = new Environment(funcionOriginal.closure);

        // Declarar receptor como 'this'
        newEnv.DeclareVariable("this", new InstanceValue(instancia), typeof(InstanceValue));
        Console.WriteLine($"[DEBUG] ID().Length = {funcionOriginal.context.ID().Length}");

        if (funcionOriginal.context.ID().Length >= 3)
        {
            string receptorName = funcionOriginal.context.ID(0).GetText(); 
            newEnv.DeclareVariable(receptorName, new InstanceValue(instancia), typeof(InstanceValue));
            Console.WriteLine($"[DEBUG] Variable {receptorName} registrada en el entorno del metodo");
        }

        // Declarar parámetros normales
        var parametros = funcionOriginal.context.@params()?.param();
        if (parametros != null)
        {
            for (int i = 0; i < parametros.Length; i++)
            {
                string id = parametros[i].ID().GetText();
                newEnv.DeclareVariable(id, args[i], args[i].GetType());
            }
        }

var anterior = visitor.currentEnvironment;
visitor.currentEnvironment = newEnv;

try
{
    foreach (var instr in funcionOriginal.context.listainstrucciones())
    {
        visitor.Visit(instr);
    }
}
catch (ReturnException ex)
{
    visitor.currentEnvironment = anterior;
    return ex.Value;
}

visitor.currentEnvironment = anterior;
return visitor.defaultValue;
}
}
