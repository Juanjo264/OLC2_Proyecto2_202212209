// Generated from /home/juanjo/compi2/OLC2_Proyecto2_202212209/Proyecto 2/api/Language.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link LanguageParser}.
 */
public interface LanguageListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link LanguageParser#program}.
	 * @param ctx the parse tree
	 */
	void enterProgram(LanguageParser.ProgramContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#program}.
	 * @param ctx the parse tree
	 */
	void exitProgram(LanguageParser.ProgramContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#listainstrucciones}.
	 * @param ctx the parse tree
	 */
	void enterListainstrucciones(LanguageParser.ListainstruccionesContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#listainstrucciones}.
	 * @param ctx the parse tree
	 */
	void exitListainstrucciones(LanguageParser.ListainstruccionesContext ctx);
	/**
	 * Enter a parse tree produced by the {@code declaracionVar}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void enterDeclaracionVar(LanguageParser.DeclaracionVarContext ctx);
	/**
	 * Exit a parse tree produced by the {@code declaracionVar}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void exitDeclaracionVar(LanguageParser.DeclaracionVarContext ctx);
	/**
	 * Enter a parse tree produced by the {@code declaracionImplicita}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void enterDeclaracionImplicita(LanguageParser.DeclaracionImplicitaContext ctx);
	/**
	 * Exit a parse tree produced by the {@code declaracionImplicita}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void exitDeclaracionImplicita(LanguageParser.DeclaracionImplicitaContext ctx);
	/**
	 * Enter a parse tree produced by the {@code declaracionSlicevacio}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void enterDeclaracionSlicevacio(LanguageParser.DeclaracionSlicevacioContext ctx);
	/**
	 * Exit a parse tree produced by the {@code declaracionSlicevacio}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void exitDeclaracionSlicevacio(LanguageParser.DeclaracionSlicevacioContext ctx);
	/**
	 * Enter a parse tree produced by the {@code declaracionSlice}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void enterDeclaracionSlice(LanguageParser.DeclaracionSliceContext ctx);
	/**
	 * Exit a parse tree produced by the {@code declaracionSlice}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void exitDeclaracionSlice(LanguageParser.DeclaracionSliceContext ctx);
	/**
	 * Enter a parse tree produced by the {@code declaracionSlicemulti}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void enterDeclaracionSlicemulti(LanguageParser.DeclaracionSlicemultiContext ctx);
	/**
	 * Exit a parse tree produced by the {@code declaracionSlicemulti}
	 * labeled alternative in {@link LanguageParser#variables}.
	 * @param ctx the parse tree
	 */
	void exitDeclaracionSlicemulti(LanguageParser.DeclaracionSlicemultiContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#fila}.
	 * @param ctx the parse tree
	 */
	void enterFila(LanguageParser.FilaContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#fila}.
	 * @param ctx the parse tree
	 */
	void exitFila(LanguageParser.FilaContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#structdcl}.
	 * @param ctx the parse tree
	 */
	void enterStructdcl(LanguageParser.StructdclContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#structdcl}.
	 * @param ctx the parse tree
	 */
	void exitStructdcl(LanguageParser.StructdclContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#structBody}.
	 * @param ctx the parse tree
	 */
	void enterStructBody(LanguageParser.StructBodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#structBody}.
	 * @param ctx the parse tree
	 */
	void exitStructBody(LanguageParser.StructBodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#funcdlc}.
	 * @param ctx the parse tree
	 */
	void enterFuncdlc(LanguageParser.FuncdlcContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#funcdlc}.
	 * @param ctx the parse tree
	 */
	void exitFuncdlc(LanguageParser.FuncdlcContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#params}.
	 * @param ctx the parse tree
	 */
	void enterParams(LanguageParser.ParamsContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#params}.
	 * @param ctx the parse tree
	 */
	void exitParams(LanguageParser.ParamsContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#param}.
	 * @param ctx the parse tree
	 */
	void enterParam(LanguageParser.ParamContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#param}.
	 * @param ctx the parse tree
	 */
	void exitParam(LanguageParser.ParamContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ExprecionInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterExprecionInstruccion(LanguageParser.ExprecionInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ExprecionInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitExprecionInstruccion(LanguageParser.ExprecionInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code BreakInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterBreakInstruccion(LanguageParser.BreakInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code BreakInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitBreakInstruccion(LanguageParser.BreakInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ContinueInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterContinueInstruccion(LanguageParser.ContinueInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ContinueInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitContinueInstruccion(LanguageParser.ContinueInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ReturnInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterReturnInstruccion(LanguageParser.ReturnInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ReturnInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitReturnInstruccion(LanguageParser.ReturnInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code PrintInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterPrintInstruccion(LanguageParser.PrintInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code PrintInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitPrintInstruccion(LanguageParser.PrintInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Assign}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterAssign(LanguageParser.AssignContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Assign}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitAssign(LanguageParser.AssignContext ctx);
	/**
	 * Enter a parse tree produced by the {@code BloqueInstrucciones}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterBloqueInstrucciones(LanguageParser.BloqueInstruccionesContext ctx);
	/**
	 * Exit a parse tree produced by the {@code BloqueInstrucciones}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitBloqueInstrucciones(LanguageParser.BloqueInstruccionesContext ctx);
	/**
	 * Enter a parse tree produced by the {@code IfInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterIfInstruccion(LanguageParser.IfInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code IfInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitIfInstruccion(LanguageParser.IfInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code WhileInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterWhileInstruccion(LanguageParser.WhileInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code WhileInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitWhileInstruccion(LanguageParser.WhileInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code SwitchInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterSwitchInstruccion(LanguageParser.SwitchInstruccionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code SwitchInstruccion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitSwitchInstruccion(LanguageParser.SwitchInstruccionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ForCondicion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterForCondicion(LanguageParser.ForCondicionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ForCondicion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitForCondicion(LanguageParser.ForCondicionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Forincicializacion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterForincicializacion(LanguageParser.ForincicializacionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Forincicializacion}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitForincicializacion(LanguageParser.ForincicializacionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ForRange}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void enterForRange(LanguageParser.ForRangeContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ForRange}
	 * labeled alternative in {@link LanguageParser#instruccion}.
	 * @param ctx the parse tree
	 */
	void exitForRange(LanguageParser.ForRangeContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#cases}.
	 * @param ctx the parse tree
	 */
	void enterCases(LanguageParser.CasesContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#cases}.
	 * @param ctx the parse tree
	 */
	void exitCases(LanguageParser.CasesContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#defaultCase}.
	 * @param ctx the parse tree
	 */
	void enterDefaultCase(LanguageParser.DefaultCaseContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#defaultCase}.
	 * @param ctx the parse tree
	 */
	void exitDefaultCase(LanguageParser.DefaultCaseContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Equalitys}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterEqualitys(LanguageParser.EqualitysContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Equalitys}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitEqualitys(LanguageParser.EqualitysContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Callee}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterCallee(LanguageParser.CalleeContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Callee}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitCallee(LanguageParser.CalleeContext ctx);
	/**
	 * Enter a parse tree produced by the {@code New}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterNew(LanguageParser.NewContext ctx);
	/**
	 * Exit a parse tree produced by the {@code New}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitNew(LanguageParser.NewContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Logicos}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterLogicos(LanguageParser.LogicosContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Logicos}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitLogicos(LanguageParser.LogicosContext ctx);
	/**
	 * Enter a parse tree produced by the {@code IncrementoDecremento}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterIncrementoDecremento(LanguageParser.IncrementoDecrementoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code IncrementoDecremento}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitIncrementoDecremento(LanguageParser.IncrementoDecrementoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Mod}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterMod(LanguageParser.ModContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Mod}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitMod(LanguageParser.ModContext ctx);
	/**
	 * Enter a parse tree produced by the {@code boleanTrueExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterBoleanTrueExpresion(LanguageParser.BoleanTrueExpresionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code boleanTrueExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitBoleanTrueExpresion(LanguageParser.BoleanTrueExpresionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code InstanciaStruct}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterInstanciaStruct(LanguageParser.InstanciaStructContext ctx);
	/**
	 * Exit a parse tree produced by the {@code InstanciaStruct}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitInstanciaStruct(LanguageParser.InstanciaStructContext ctx);
	/**
	 * Enter a parse tree produced by the {@code MulDiv}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterMulDiv(LanguageParser.MulDivContext ctx);
	/**
	 * Exit a parse tree produced by the {@code MulDiv}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitMulDiv(LanguageParser.MulDivContext ctx);
	/**
	 * Enter a parse tree produced by the {@code AddSub}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterAddSub(LanguageParser.AddSubContext ctx);
	/**
	 * Exit a parse tree produced by the {@code AddSub}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitAddSub(LanguageParser.AddSubContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Parens}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterParens(LanguageParser.ParensContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Parens}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitParens(LanguageParser.ParensContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Relational}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterRelational(LanguageParser.RelationalContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Relational}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitRelational(LanguageParser.RelationalContext ctx);
	/**
	 * Enter a parse tree produced by the {@code operadorNegacion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterOperadorNegacion(LanguageParser.OperadorNegacionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code operadorNegacion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitOperadorNegacion(LanguageParser.OperadorNegacionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Int}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterInt(LanguageParser.IntContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Int}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitInt(LanguageParser.IntContext ctx);
	/**
	 * Enter a parse tree produced by the {@code caracterExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterCaracterExpresion(LanguageParser.CaracterExpresionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code caracterExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitCaracterExpresion(LanguageParser.CaracterExpresionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code nilExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterNilExpresion(LanguageParser.NilExpresionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code nilExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitNilExpresion(LanguageParser.NilExpresionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code ModuleFuncCall}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterModuleFuncCall(LanguageParser.ModuleFuncCallContext ctx);
	/**
	 * Exit a parse tree produced by the {@code ModuleFuncCall}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitModuleFuncCall(LanguageParser.ModuleFuncCallContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Float}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterFloat(LanguageParser.FloatContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Float}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitFloat(LanguageParser.FloatContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Idexpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterIdexpresion(LanguageParser.IdexpresionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Idexpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitIdexpresion(LanguageParser.IdexpresionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code AccesoSlice}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterAccesoSlice(LanguageParser.AccesoSliceContext ctx);
	/**
	 * Exit a parse tree produced by the {@code AccesoSlice}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitAccesoSlice(LanguageParser.AccesoSliceContext ctx);
	/**
	 * Enter a parse tree produced by the {@code cadenaExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterCadenaExpresion(LanguageParser.CadenaExpresionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code cadenaExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitCadenaExpresion(LanguageParser.CadenaExpresionContext ctx);
	/**
	 * Enter a parse tree produced by the {@code AccesoSliceMulti}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterAccesoSliceMulti(LanguageParser.AccesoSliceMultiContext ctx);
	/**
	 * Exit a parse tree produced by the {@code AccesoSliceMulti}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitAccesoSliceMulti(LanguageParser.AccesoSliceMultiContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Negate}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterNegate(LanguageParser.NegateContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Negate}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitNegate(LanguageParser.NegateContext ctx);
	/**
	 * Enter a parse tree produced by the {@code boleanFalseExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void enterBoleanFalseExpresion(LanguageParser.BoleanFalseExpresionContext ctx);
	/**
	 * Exit a parse tree produced by the {@code boleanFalseExpresion}
	 * labeled alternative in {@link LanguageParser#expr}.
	 * @param ctx the parse tree
	 */
	void exitBoleanFalseExpresion(LanguageParser.BoleanFalseExpresionContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#camposStruct}.
	 * @param ctx the parse tree
	 */
	void enterCamposStruct(LanguageParser.CamposStructContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#camposStruct}.
	 * @param ctx the parse tree
	 */
	void exitCamposStruct(LanguageParser.CamposStructContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#campoStruct}.
	 * @param ctx the parse tree
	 */
	void enterCampoStruct(LanguageParser.CampoStructContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#campoStruct}.
	 * @param ctx the parse tree
	 */
	void exitCampoStruct(LanguageParser.CampoStructContext ctx);
	/**
	 * Enter a parse tree produced by the {@code FuncCall}
	 * labeled alternative in {@link LanguageParser#call}.
	 * @param ctx the parse tree
	 */
	void enterFuncCall(LanguageParser.FuncCallContext ctx);
	/**
	 * Exit a parse tree produced by the {@code FuncCall}
	 * labeled alternative in {@link LanguageParser#call}.
	 * @param ctx the parse tree
	 */
	void exitFuncCall(LanguageParser.FuncCallContext ctx);
	/**
	 * Enter a parse tree produced by the {@code Get}
	 * labeled alternative in {@link LanguageParser#call}.
	 * @param ctx the parse tree
	 */
	void enterGet(LanguageParser.GetContext ctx);
	/**
	 * Exit a parse tree produced by the {@code Get}
	 * labeled alternative in {@link LanguageParser#call}.
	 * @param ctx the parse tree
	 */
	void exitGet(LanguageParser.GetContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#args}.
	 * @param ctx the parse tree
	 */
	void enterArgs(LanguageParser.ArgsContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#args}.
	 * @param ctx the parse tree
	 */
	void exitArgs(LanguageParser.ArgsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code incremento}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void enterIncremento(LanguageParser.IncrementoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code incremento}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void exitIncremento(LanguageParser.IncrementoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code asignarSlice}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void enterAsignarSlice(LanguageParser.AsignarSliceContext ctx);
	/**
	 * Exit a parse tree produced by the {@code asignarSlice}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void exitAsignarSlice(LanguageParser.AsignarSliceContext ctx);
	/**
	 * Enter a parse tree produced by the {@code AsignarSliceMulti}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void enterAsignarSliceMulti(LanguageParser.AsignarSliceMultiContext ctx);
	/**
	 * Exit a parse tree produced by the {@code AsignarSliceMulti}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void exitAsignarSliceMulti(LanguageParser.AsignarSliceMultiContext ctx);
	/**
	 * Enter a parse tree produced by the {@code asignarSliceCompleto}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void enterAsignarSliceCompleto(LanguageParser.AsignarSliceCompletoContext ctx);
	/**
	 * Exit a parse tree produced by the {@code asignarSliceCompleto}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void exitAsignarSliceCompleto(LanguageParser.AsignarSliceCompletoContext ctx);
	/**
	 * Enter a parse tree produced by the {@code asignarVar}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void enterAsignarVar(LanguageParser.AsignarVarContext ctx);
	/**
	 * Exit a parse tree produced by the {@code asignarVar}
	 * labeled alternative in {@link LanguageParser#asignacion}.
	 * @param ctx the parse tree
	 */
	void exitAsignarVar(LanguageParser.AsignarVarContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#print}.
	 * @param ctx the parse tree
	 */
	void enterPrint(LanguageParser.PrintContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#print}.
	 * @param ctx the parse tree
	 */
	void exitPrint(LanguageParser.PrintContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#impresiones}.
	 * @param ctx the parse tree
	 */
	void enterImpresiones(LanguageParser.ImpresionesContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#impresiones}.
	 * @param ctx the parse tree
	 */
	void exitImpresiones(LanguageParser.ImpresionesContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#if}.
	 * @param ctx the parse tree
	 */
	void enterIf(LanguageParser.IfContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#if}.
	 * @param ctx the parse tree
	 */
	void exitIf(LanguageParser.IfContext ctx);
	/**
	 * Enter a parse tree produced by {@link LanguageParser#tipo}.
	 * @param ctx the parse tree
	 */
	void enterTipo(LanguageParser.TipoContext ctx);
	/**
	 * Exit a parse tree produced by {@link LanguageParser#tipo}.
	 * @param ctx the parse tree
	 */
	void exitTipo(LanguageParser.TipoContext ctx);
}