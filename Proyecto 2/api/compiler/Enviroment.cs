public class Environment {
  public Dictionary<string, ValueWrapper> variables = new Dictionary<string, ValueWrapper>();

public Environment? parent;

public Environment (Environment? parent ) {

    this.parent = parent;

}

  public ValueWrapper GetVariable(string id, Antlr4.Runtime.IToken token) {
    if (variables.ContainsKey(id)) {
      return variables[id];
    }
    if (parent != null) {
      return parent.GetVariable(id, token);
    }
    throw new SemanticError("Variable " + id + " not found",token );
  }

public void DeclareVariable(string id, ValueWrapper value, Type tipoEsperado = null!, Antlr4.Runtime.IToken? token = null) {
    if (variables.ContainsKey(id)) {
        throw new SemanticError("Error: La variable " + id + " ya ha sido declarada.", token ?? new Antlr4.Runtime.CommonToken(0));
    }

    if (value == null) {
        if (tipoEsperado == null) {
            throw new SemanticError("Error: No se proporcionó un tipo esperado para la variable " + id, token ?? new Antlr4.Runtime.CommonToken(0));
        }
        value = GetDefaultValue(tipoEsperado);
    }

    // Asignar la variable en el entorno
    variables[id] = value;

    string dataType = tipoEsperado?.Name ?? value?.GetType().Name ?? "Desconocido";

    int line = token?.Line ?? 0;
    int column = token?.Column ?? 0;

    if (CompilerVisitor.GlobalSymbolTable != null) {
        string scope = this.parent == null ? "Global" : "Local";
        CompilerVisitor.GlobalSymbolTable.AddSymbol(id, "Variable", dataType, scope, line, column);
        Console.WriteLine(" Variable " +id +"registrada en la tabla de símbolos con tipo ", dataType , "en línea " ,line," columna" ,column);
    } else {
        Console.WriteLine(" No se pudo registrar la variable "+id +" porque la tabla de símbolos no está inicializada.");
    }
}
public ValueWrapper GetDefaultValue(Type tipoEsperado) {
    if (tipoEsperado == typeof(IntValue)) {
        return new IntValue(0);
    } else if (tipoEsperado == typeof(FloatValue)) {
        return new FloatValue((float)0.0);
    } else if (tipoEsperado == typeof(BoolValue)) {
        return new BoolValue(false);
    } else if (tipoEsperado == typeof(StringValue)) {
        return new StringValue("");
    } else if (tipoEsperado == typeof(RuneValue)) {
        return new RuneValue('\0');
    }
     else {
        throw new SemanticError("Error: Tipo desconocido " + tipoEsperado, new Antlr4.Runtime.CommonToken(0));
    }
}

public void AssignVariable(string id, ValueWrapper value, Antlr4.Runtime.IToken? token) {
    if (variables.ContainsKey(id)) {
        ValueWrapper valorAntiguo = variables[id];

        // Manejo especial de Slices
        if (valorAntiguo is SliceValue oldSlice && value is SliceValue newSlice) {
            if (oldSlice.Values.Count == 0 || newSlice.Values.Count == 0) {
                variables[id] = value;
                return;
            }

            Type? tipoSliceViejo = oldSlice.Values.Count > 0 ? oldSlice.Values[0].GetType() : null;
            Type? tipoSliceNuevo = newSlice.Values.Count > 0 ? newSlice.Values[0].GetType() : null;

            if (tipoSliceViejo != null && tipoSliceNuevo != null && tipoSliceViejo != tipoSliceNuevo) {
                throw new SemanticError($"Error: No se puede asignar un slice de {tipoSliceNuevo} a un slice de {tipoSliceViejo}.", 
                                        token ?? throw new Exception($"Error interno: No se recibió un token válido al asignar '{id}'.")); 
            }

            variables[id] = value;
            return;
        }

        // Validar si la asignación es válida
        if (valorAntiguo.GetType() == value.GetType() || 
            (valorAntiguo is FloatValue && value is IntValue)) {
            variables[id] = value;
            return;
        } else {
            throw new SemanticError("Error: No se puede cambiar el tipo de la variable " + id, token ?? throw new Exception($"Error interno: No se recibió un token válido al asignar '{id}'.")); 
        }
    }

    if (parent != null) {
        parent.AssignVariable(id, value, token);
        return;
    }

    throw new SemanticError("Error: La variable " + id + " no ha sido declarada.", token ?? throw new Exception($"Error interno: No se recibió un token válido al asignar '{id}'.")); 
}
}