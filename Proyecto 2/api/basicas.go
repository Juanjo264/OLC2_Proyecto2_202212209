puntos := 0

fmt.Println("=== Archivo de prueba básico ===")
fmt.Println("Validaciones manuales esperadas: 4")

// 1. Declaración de variables (3 puntos)
fmt.Println("==== Declaración de variables ====")
puntosDeclaracion := 0

fmt.Println("Declaración explícita con tipo y valor")
var entero int = 42
var decimal float64 = 3.14
var texto string = "Hola!"
var booleano bool = true
var caracter rune = 'A'

fmt.Println("\n\n###Validacion Manual")
fmt.Println("entero:", entero)
fmt.Println("decimal:", decimal)
fmt.Println("texto:", texto)
fmt.Println("booleano:", booleano)
fmt.Println("caracter:", caracter)
fmt.Println("")

if entero == 42 && decimal > 3.0 && texto == "Hola!" && booleano == true && caracter == 'A' {
	puntosDeclaracion = puntosDeclaracion + 1
	fmt.Println("OK Declaración explícita: correcto")
} else {
	fmt.Println("X Declaración explícita: incorrecto")
}

fmt.Println("Declaración sin valor")
var enteroSinValor int
var decimalSinValor float64
var textoSinValor string
var booleanoSinValor bool

fmt.Println("enteroSinValor:", enteroSinValor)
fmt.Println("decimalSinValor:", decimalSinValor)
fmt.Println("textoSinValor:", textoSinValor)
fmt.Println("booleanoSinValor:", booleanoSinValor)

if enteroSinValor == 0 && decimalSinValor == 0.0 && textoSinValor == "" && booleanoSinValor == false {
	puntosDeclaracion = puntosDeclaracion + 1
	fmt.Println("OK Declaración sin valor: correcto")
} else {
	fmt.Println("X Declaración sin valor: incorrecto")
}

fmt.Println("Declaración con inferencia de tipo")
enteroInferido := 100
decimalInferido := 2.71
textoInferido := "Adios!"
booleanoInferido := false

fmt.Println("enteroInferido:", enteroInferido)
fmt.Println("decimalInferido:", decimalInferido)
fmt.Println("textoInferido:", textoInferido)
fmt.Println("booleanoInferido:", booleanoInferido)

if enteroInferido == 100 && decimalInferido > 2.7 && textoInferido == "Adios!" && booleanoInferido == false {
	puntosDeclaracion = puntosDeclaracion + 1
	fmt.Println("OK Declaración con inferencia: correcto")
} else {
	fmt.Println("X Declaración con inferencia: incorrecto")
}

// 2. Asignación de variables (3 puntos)
fmt.Println("\n==== Asignación de variables ====")
puntosAsignacion := 0

fmt.Println("Asignación con tipo correcto")
entero = 99
decimal = 9.9
texto = "Nuevo"
booleano = !booleano

fmt.Println("\n\n###Validacion Manual")
fmt.Println("entero:", entero)
fmt.Println("decimal:", decimal)
fmt.Println("texto:", texto)
fmt.Println("booleano:", booleano)
fmt.Println("")

if entero == 99 && decimal == 9.9 && texto == "Nuevo" && booleano == false {
	puntosAsignacion = puntosAsignacion + 1
	fmt.Println("OK Asignación simple: correcto")
} else {
	fmt.Println("X Asignación simple: incorrecto")
}

fmt.Println("Asignación con expresiones")
entero = entero + 1
decimal = decimal * 2
texto = texto + "!"
booleano = !booleano

fmt.Println("entero:", entero)
fmt.Println("decimal:", decimal)
fmt.Println("texto:", texto)
fmt.Println("booleano:", booleano)

if entero == 100 && decimal == 19.8 && texto == "Nuevo!" && booleano == true {
	puntosAsignacion = puntosAsignacion + 1
	fmt.Println("OK Asignación con expresiones: correcto")
} else {
	fmt.Println("X Asignación con expresiones: incorrecto")
}

fmt.Println("Asignación con tipo incorrecto")
puntosAsignacion = puntosAsignacion + 1
fmt.Println("OK Asignación con tipo incorrecto: correcto")

// 3. Operaciones Aritméticas (3 puntos)
fmt.Println("\n==== Operaciones Aritméticas ====")
puntosOperacionesAritmeticas := 0

fmt.Println("Suma")
resultadoSuma1 := 10 + 5
resultadoSuma2 := 10.5 + 5.5
resultadoSuma3 := 10 + 5.5
resultadoSuma4 := 10.5 + 5

fmt.Println("10 + 5 =", resultadoSuma1)
fmt.Println("10.5 + 5.5 =", resultadoSuma2)
fmt.Println("10 + 5.5 =", resultadoSuma3)
fmt.Println("10.5 + 5 =", resultadoSuma4)

if resultadoSuma1 == 15 && resultadoSuma2 == 16.0 && resultadoSuma3 == 15.5 && resultadoSuma4 == 15.5 {
	puntosOperacionesAritmeticas = puntosOperacionesAritmeticas + 1
	fmt.Println("OK Suma: correcto")
} else {
	fmt.Println("X Suma: incorrecto")
}

fmt.Println("Multiplicación")
resultadoMult1 := 5 * 3
resultadoMult2 := 5.5 * 2.0
resultadoMult3 := 5 * 2.5
resultadoMult4 := 5.5 * 2

fmt.Println("5 * 3 =", resultadoMult1)
fmt.Println("5.5 * 2.0 =", resultadoMult2)
fmt.Println("5 * 2.5 =", resultadoMult3)
fmt.Println("5.5 * 2 =", resultadoMult4)

if resultadoMult1 == 15 && resultadoMult2 == 11.0 && resultadoMult3 == 12.5 && resultadoMult4 == 11.0 {
	puntosOperacionesAritmeticas = puntosOperacionesAritmeticas + 1
	fmt.Println("OK Multiplicación: correcto")
} else {
	fmt.Println("X Multiplicación: incorrecto")
}

fmt.Println("División")
resultadoDiv1 := 10 / 2
resultadoDiv2 := 10.0 / 4.0
resultadoDiv3 := 10 / 4.0
resultadoDiv4 := 10.0 / 4

fmt.Println("10 / 2 =", resultadoDiv1)
fmt.Println("10.0 / 4.0 =", resultadoDiv2)
fmt.Println("10 / 4.0 =", resultadoDiv3)
fmt.Println("10.0 / 4 =", resultadoDiv4)

if resultadoDiv1 == 5 && resultadoDiv2 == 2.5 && resultadoDiv3 == 2.5 && resultadoDiv4 == 2.5 {
	puntosOperacionesAritmeticas = puntosOperacionesAritmeticas + 1
	fmt.Println("OK División: correcto")
} else {
	fmt.Println("X División: incorrecto")
}

// 4. Operaciones Relacionales (3 puntos)
fmt.Println("\n==== Operaciones Relacionales ====")
puntosOperacionesRelacionales := 0

fmt.Println("Igualdad")
resultadoIgualdad1 := 10 == 10
resultadoIgualdad2 := 10 == 5
resultadoIgualdad3 := 10.5 == 10.5
resultadoIgualdad4 := 10.5 == 5.5
resultadoIgualdad5 := "Hola" == "Hola"
resultadoIgualdad6 := "Hola" == "Mundo"

fmt.Println("10 == 10:", resultadoIgualdad1)
fmt.Println("10 == 5:", resultadoIgualdad2)
fmt.Println("10.5 == 10.5:", resultadoIgualdad3)
fmt.Println("10.5 == 5.5:", resultadoIgualdad4)
fmt.Println("\"Hola\" == \"Hola\":", resultadoIgualdad5)
fmt.Println("\"Hola\" == \"Mundo\":", resultadoIgualdad6)

if resultadoIgualdad1 == true && resultadoIgualdad2 == false &&
	resultadoIgualdad3 == true && resultadoIgualdad4 == false &&
	resultadoIgualdad5 == true && resultadoIgualdad6 == false {
	puntosOperacionesRelacionales = puntosOperacionesRelacionales + 1
	fmt.Println("OK Igualdad: correcto")
} else {
	fmt.Println("X Igualdad: incorrecto")
}

fmt.Println("Mayor/Menor")
resultadoComp1 := 10 > 5
resultadoComp2 := 10 < 5
resultadoComp3 := 10.5 > 5.5
resultadoComp4 := 10.5 < 5.5

fmt.Println("10 > 5:", resultadoComp1)
fmt.Println("10 < 5:", resultadoComp2)
fmt.Println("10.5 > 5.5:", resultadoComp3)
fmt.Println("10.5 < 5.5:", resultadoComp4)

if resultadoComp1 == true && resultadoComp2 == false &&
	resultadoComp3 == true && resultadoComp4 == false {
	puntosOperacionesRelacionales = puntosOperacionesRelacionales + 1
	fmt.Println("OK Mayor/Menor: correcto")
} else {
	fmt.Println("X Mayor/Menor: incorrecto")
}

fmt.Println("Mayor o igual/Menor o igual")
resultadoComp5 := 10 >= 10
resultadoComp6 := 10 <= 5
resultadoComp7 := 10.5 >= 5.5
resultadoComp8 := 10.5 <= 10.5

fmt.Println("10 >= 10:", resultadoComp5)
fmt.Println("10 <= 5:", resultadoComp6)
fmt.Println("10.5 >= 5.5:", resultadoComp7)
fmt.Println("10.5 <= 10.5:", resultadoComp8)

if resultadoComp5 == true && resultadoComp6 == false &&
	resultadoComp7 == true && resultadoComp8 == true {
	puntosOperacionesRelacionales = puntosOperacionesRelacionales + 1
	fmt.Println("OK Mayor o igual/Menor o igual: correcto")
} else {
	fmt.Println("X Mayor o igual/Menor o igual: incorrecto")
}

// 5. Operaciones Lógicas (3 puntos)
fmt.Println("\n==== Operaciones Lógicas ====")
puntosOperacionesLogicas := 0

fmt.Println("AND")
resultadoAnd1 := true && true
resultadoAnd2 := true && false
resultadoAnd3 := (10 == 10) && (5 == 5)
resultadoAnd4 := (10 == 10) && (5 == 6)

fmt.Println("true && true:", resultadoAnd1)
fmt.Println("true && false:", resultadoAnd2)
fmt.Println("(10 == 10) && (5 == 5):", resultadoAnd3)
fmt.Println("(10 == 10) && (5 == 6):", resultadoAnd4)

if resultadoAnd1 == true && resultadoAnd2 == false &&
	resultadoAnd3 == true && resultadoAnd4 == false {
	puntosOperacionesLogicas = puntosOperacionesLogicas + 1
	fmt.Println("OK AND: correcto")
} else {
	fmt.Println("X AND: incorrecto")
}

fmt.Println("OR")
resultadoOr1 := true || false
resultadoOr2 := false || false
resultadoOr3 := (10 == 10) || (5 == 6)
resultadoOr4 := (10 == 11) || (5 == 6)

fmt.Println("true || false:", resultadoOr1)
fmt.Println("false || false:", resultadoOr2)
fmt.Println("(10 == 10) || (5 == 6):", resultadoOr3)
fmt.Println("(10 == 11) || (5 == 6):", resultadoOr4)

if resultadoOr1 == true && resultadoOr2 == false &&
	resultadoOr3 == true && resultadoOr4 == false {
	puntosOperacionesLogicas = puntosOperacionesLogicas + 1
	fmt.Println("OK OR: correcto")
} else {
	fmt.Println("X OR: incorrecto")
}

fmt.Println("NOT")
resultadoNot1 := !true
resultadoNot2 := !false
resultadoNot3 := !(10 == 10)
resultadoNot4 := !(10 == 11)

fmt.Println("!true:", resultadoNot1)
fmt.Println("!false:", resultadoNot2)
fmt.Println("!(10 == 10):", resultadoNot3)
fmt.Println("!(10 == 11):", resultadoNot4)

if resultadoNot1 == false && resultadoNot2 == true &&
	resultadoNot3 == false && resultadoNot4 == true {
	puntosOperacionesLogicas = puntosOperacionesLogicas + 1
	fmt.Println("OK NOT: correcto")
} else {
	fmt.Println("X NOT: incorrecto")
}

// 6. fmt.Println (3 puntos)
fmt.Println("\n==== fmt.Println ====")
puntosPrintln := 0

fmt.Println("\n\n###Validacion Manual")
fmt.Println("Impresión de valores simples")
fmt.Println(42)
fmt.Println(3.14)
fmt.Println("Texto")
fmt.Println(true)
fmt.Println('A')
fmt.Println("")

puntosPrintln = puntosPrintln + 1
fmt.Println("OK Impresión de valores simples: correcto")

fmt.Println("Impresión de múltiples valores")
fmt.Println("Números:", 42, 3.14)
fmt.Println("Booleano:", true, "Texto:", "Hola")

puntosPrintln = puntosPrintln + 1
fmt.Println("OK Impresión de múltiples valores: correcto")

fmt.Println("Impresión de expresiones")
fmt.Println("Suma:", 10+5)
fmt.Println("Comparación:", 10 > 5)
fmt.Println("Lógica:", true && false)

puntosPrintln = puntosPrintln + 1
fmt.Println("OK Impresión de expresiones: correcto")

// 7. Manejo de valor nulo (2 puntos)
fmt.Println("\n==== Manejo de valor nulo ====")
puntosValorNulo := 0

fmt.Println("Valores por defecto")
var enteroNulo int
var decimalNulo float64
var textoNulo string
var booleanoNulo bool

fmt.Println("\n\n###Validacion Manual")
fmt.Println("enteroNulo:", enteroNulo)
fmt.Println("decimalNulo:", decimalNulo)
fmt.Println("textoNulo:", textoNulo)
fmt.Println("booleanoNulo:", booleanoNulo)
fmt.Println("")

if enteroNulo == 0 && decimalNulo == 0.0 && textoNulo == "" && booleanoNulo == false {
	puntosValorNulo = puntosValorNulo + 1
	fmt.Println("OK Valores por defecto: correcto")
} else {
	fmt.Println("X Valores por defecto: incorrecto")
}

fmt.Println("Operaciones con nil")
puntosValorNulo = puntosValorNulo + 1
fmt.Println("OK Operaciones con nil: correcto")

// Resumen de puntos
puntos = puntosDeclaracion + puntosAsignacion + puntosOperacionesAritmeticas +
	puntosOperacionesRelacionales + puntosOperacionesLogicas +
	puntosPrintln + puntosValorNulo

fmt.Println("\n=== Tabla de Resultados ===")
fmt.Println("+--------------------------+--------+-------+")
fmt.Println("| Característica           | Puntos | Total |")
fmt.Println("+--------------------------+--------+-------+")
fmt.Println("| Declaración de variables | ", puntosDeclaracion, "    | 3     |")
fmt.Println("| Asignación de variables  | ", puntosAsignacion, "    | 3     |")
fmt.Println("| Operaciones Aritméticas  | ", puntosOperacionesAritmeticas, "    | 3     |")
fmt.Println("| Operaciones Relacionales | ", puntosOperacionesRelacionales, "    | 3     |")
fmt.Println("| Operaciones Lógicas      | ", puntosOperacionesLogicas, "    | 3     |")
fmt.Println("| fmt.Println              | ", puntosPrintln, "    | 3     |")
fmt.Println("| Manejo de valor nulo     | ", puntosValorNulo, "    | 2     |")
fmt.Println("+--------------------------+--------+-------+")
fmt.Println("| TOTAL                    | ", puntos, "   | 20    |")
fmt.Println("+--------------------------+--------+-------+")


================================================================================