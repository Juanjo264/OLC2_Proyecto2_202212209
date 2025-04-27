grammar Language;

options { caseInsensitive = false; }

INT: [0-9]+;
BOOL: 'true' | 'false';
FLOAT: [0-9]+'.'[0-9]+;
WS: [ \t\r\n]+ -> channel(HIDDEN);
CARACTER : '\'' . '\'' ; 
CADENA: '"' (ESC_SEQ | ~["\\\r\n])* '"' ; 
fragment ESC_SEQ: '\\' [btnr"\\];
ID: [a-zA-Z_][a-zA-Z0-9]*;


PIZQ: '(';
PDER: ')';
LLAVE_ABRE: '{';
LLAVE_CIERRA: '}';
CORCHETE_ABRE: '[';
CORCHETE_CIERRA: ']';

IGUAL: '=';
DOSPUNTOS_IGUAL: ':=';
MASIGUAL: '+=';
MENOSIGUAL: '-=';

LLINEAC: '//' ~[\r\n]* -> skip;
MULTILINEAC: '/*' .*? '*/' -> skip;


program: listainstrucciones* ;

listainstrucciones : variables |instruccion | structdcl | funcdlc ;

variables: 'var' ID tipo '=' expr ';';


// variables: 'var' ID tipo (IGUAL expr)?  #declaracionVar 
// | ID DOSPUNTOS_IGUAL expr      #declaracionImplicita 
// | 'var' ID '[]' tipo #declaracionSlicevacio
// | ID DOSPUNTOS_IGUAL '[]' tipo '{' expr (','expr)* '}'  #declaracionSlice
// | ID DOSPUNTOS_IGUAL '[][]' tipo '{' fila (',' fila)* ','? '}' #declaracionSlicemulti
// ;



fila: '{' expr (',' expr)* ','?'}' ;

structdcl: 'type' ID 'struct' '{' structBody* '}' ;

structBody: variables | funcdlc | ID tipo ;
 

funcdlc:  'func' '(' ID ID ')' ID '(' params? ')' tipo? '{' listainstrucciones* '}'  
| 'func' ID '(' params? ')' tipo? '{' listainstrucciones* '}' ;

params: param (',' param)* ;
param: ID tipo ; 


instruccion:  expr  #ExprecionInstruccion 
|'break' #BreakInstruccion
|'continue' #ContinueInstruccion
|'return' expr? #ReturnInstruccion
| print #PrintInstruccion					
| '{' listainstrucciones* '}' #BloqueInstrucciones
| 'if'  '(' expr ')' instruccion ('else' instruccion)? #IfInstruccion
| 'while' '(' expr ')' instruccion #WhileInstruccion
| 'switch'  expr  '{' cases* (defaultCase)? '}'  #SwitchInstruccion
| 'for' '(' forInit expr ';' expr ')' instruccion  #ForCondicion
| 'for' (expr | variables) ';' expr ';' expr instruccion #Forincicializacion
| 'for' ID ',' ID DOSPUNTOS_IGUAL 'range' ID instruccion #ForRange

;

forInit: variables | expr ';';


cases:
    'case' expr ':' listainstrucciones*
    ;

defaultCase:
    'default' ':' listainstrucciones*  
    ;

expr:
	'(' expr ')'			# Parens
	| expr call+  #Callee
	| ID LLAVE_ABRE camposStruct LLAVE_CIERRA   # InstanciaStruct
	| '!' right=expr #operadorNegacion
	|'-' expr                   	  # Negate
	| expr '.' ID '(' args? ')'             # ModuleFuncCall
	| expr op =  ('*' | '/') expr	# MulDiv
	| expr op = '%' expr	# Mod
	| expr  op = ('+' | '-') expr	# AddSub
	| expr	op = ('>' | '<' | '>=' | '<=' ) expr	# Relational
	| expr	op = ('==' | '!=') expr	# Equalitys
  	| expr '[' expr ']' '[' expr ']'  #AccesoSliceMulti
	| expr '[' expr ']'  #AccesoSlice
	| expr	op = ('&&' | '||' ) expr	# Logicos
	| expr '=' expr								# Assign
	| FLOAT					      # Float
	| INT					      # Int
	| 'new' ID '(' args? ')' #New
	| BOOL				# Boolean
	| CARACTER #caracterExpresion
	| CADENA #cadenaExpresion
	| ID #Idexpresion
	| ID ('++' | '--') # IncrementoDecremento 
	| 'nil' #nilExpresion
; 



camposStruct: campoStruct (',' campoStruct)* (',')? ;
campoStruct: ID ':' expr ;

//como a.b.c().f=
call: 
    '(' args? ')' 	#FuncCall 
    | '.' ID #Get;

args: expr (',' expr)* ;

// asignacion:	ID signo=(MASIGUAL | MENOSIGUAL) expr #incremento
// 					| ID '[' expr ']' IGUAL expr #asignarSlice
// 					| ID '[' expr ']' '[' expr ']' IGUAL expr  #AsignarSliceMulti
// 					| ID IGUAL '[]' tipo '{' expr (',' expr)* '}' #asignarSliceCompleto
// 					| expr IGUAL expr #asignarVar 
// 					;

print: 'print' PIZQ impresiones PDER ;

impresiones: impresiones ',' expr
	| expr;

if: 'if'  expr  LLAVE_ABRE listainstrucciones LLAVE_CIERRA ('else' LLAVE_ABRE listainstrucciones LLAVE_CIERRA)? 
  | 'if'  expr  LLAVE_ABRE listainstrucciones LLAVE_CIERRA 'else' if;

tipo: 'int'
| 'float64'
| 'string'
| 'bool'
| 'rune'
|ID
	| 'nil' 
;




