using Antlr4.Runtime.Misc;

public class FrameElement
{

  public string Name { get; set; }
  public int Offset { get; set; }
    public FrameElement(string name, int offset)
  {
    Name = name;
    Offset = offset;
  }


}

  public class FrameVisitor : LanguageBaseVisitor<Object?>
  {
    public List<FrameElement> Frame;
    public int LocalOffset;
    public int BaseOffset;

    public FrameVisitor(int baseOffset)
    {
      Frame = new List<FrameElement>();
      LocalOffset = 0;
      BaseOffset = baseOffset;
    }

public override object VisitDeclaracionVar(LanguageParser.DeclaracionVarContext context)
{
    string name = context.ID().GetText();
    Frame.Add(new FrameElement(name, BaseOffset + LocalOffset));
    LocalOffset += 1;
    return null;
}

public override object VisitDeclaracionImplicita(LanguageParser.DeclaracionImplicitaContext context)
{
    string name = context.ID().GetText();
    Frame.Add(new FrameElement(name, BaseOffset + LocalOffset));
    LocalOffset += 1;
    return null;
}


  public override object VisitBloqueInstrucciones(LanguageParser.BloqueInstruccionesContext context)
  {
    foreach (var listainstrucciones in context.listainstrucciones())
    {
      Visit(listainstrucciones);
    }
    return null;
  }

  public override object VisitIfInstruccion(LanguageParser.IfInstruccionContext context)
  {
    Visit(context.instruccion(0)); // Visitar la instrucción dentro del bloque if
    if (context.instruccion().Length > 1) Visit(context.instruccion(1)); // Visitar la instrucción dentro del bloque else
    return null;

  }

  public override object VisitWhileInstruccion( LanguageParser.WhileInstruccionContext context)
  {
    Visit(context.instruccion()); // Visitar la instrucción dentro del bloque while
    return null;
  }

  public override object VisitForincicializacion(LanguageParser.ForincicializacionContext context)
  {
    if (context.forInit().variables() != null)
    {
      Visit(context.forInit().variables()); // Visitar la declaración de variables dentro del bloque for
    }

    Visit(context.instruccion()); // Visitar la instrucción dentro del bloque for
    return null;
  } 

  public override object VisitSwitchInstruccion(LanguageParser.SwitchInstruccionContext context)
{
    foreach (var caso in context.cases())
    {
        Visit(caso);
    }

    if (context.defaultCase() != null)
    {
        Visit(context.defaultCase());
    }
    return null;
}

public override object VisitCases(LanguageParser.CasesContext context)
{
    foreach (var inst in context.listainstrucciones())
    {
        Visit(inst);
    }
    return null;
}

public override object VisitDefaultCase(LanguageParser.DefaultCaseContext context)
{
    foreach (var inst in context.listainstrucciones())
    {
        Visit(inst);
    }
    return null;
}

public override object VisitForCondicion(LanguageParser.ForCondicionContext context)
{
    Visit(context.expr()); // condición
    Visit(context.instruccion()); // cuerpo
    return null;
}

}

