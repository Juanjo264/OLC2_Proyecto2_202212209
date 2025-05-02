puntos := 0

print("=== Archivo de prueba de funcionalidades intermedias ===")
print("Validaciones manuales esperadas: 4")

// 1. Manejo de entornos (3 puntos)
print("==== Manejo de entornos ====")
puntosEntornos := 0

print("\nVariable redeclarada en un entorno diferente")
b := 10
if true {
	// Esto es válido en Go porque crea una nueva variable b en un ámbito diferente
	b := 20
	print("b dentro del if =", b)

	if b == 20 {
		puntosEntornos = puntosEntornos + 2
		print("OK Redeclaración en entorno diferente: correcto")
	} else {
		print("X Redeclaración en entorno diferente: incorrecto")
	}
}
print("b fuera del if =", b)

print("\nUso de variable en un entorno superior")
c := 10
d := 10
if true {
	// d se redefine en este ámbito
	d := 20
	// c es accesible desde el ámbito superior
	print("c dentro del if =", c)
	print("d dentro del if =", d)

	// Modificamos c del ámbito superior
	c = 30
	// Modificamos d del ámbito local
	d = 30
}
print("c fuera del if =", c)
print("d fuera del if =", d)

if c == 30 && d == 10 {
	puntosEntornos = puntosEntornos + 1
	print("OK Uso de variable en entorno superior: correcto")
} else {
	print("X Uso de variable en entorno superior: incorrecto")
}

// 2. If / Else (3 puntos)
print("\n==== If / Else ====")
puntosIfElse := 0

print("\n\n###Validacion Manual")
print("If simple")
if true {
	print("Condición verdadera")
	puntosIfElse = puntosIfElse + 1
}

print("\nIf-Else")
if true {
	print("Condición verdadera en if-else")
} else {
	print("Condición falsa en if-else")
}

if false {
	print("Esto no debería imprimirse")
} else {
	print("Condición falsa, ejecutando else")
	puntosIfElse = puntosIfElse + 1
}

print("\nIf-ElseIf-Else")
if true {
	print("Primera condición verdadera")
} else if true {
	print("Segunda condición verdadera, pero no se ejecuta")
} else {
	print("Ninguna condición verdadera")
}

if false {
	print("Primera condición falsa")
} else if true {
	print("Segunda condición verdadera")
	puntosIfElse = puntosIfElse + 1
} else {
	print("Ninguna condición verdadera")
}
print("")

// 3. For Tipo While (4 puntos)
print("\n==== For Tipo While ====")
puntosForWhile := 0

print("For como while simple")
i := 0
suma := 0
for i < 5 {
	print("i =", i)
	suma = suma + i
	i = i + 1
}

if suma == 10 {
	puntosForWhile = puntosForWhile + 1
	print("OK For como while simple: correcto")
} else {
	print("X For como while simple: incorrecto")
}

print("\nFor como while con condición compuesta")
j := 0
producto := 1
for j < 5 && producto < 50 {
	j = j + 1
	producto = producto * j
	print("j =", j, "producto =", producto)
}

if j == 5 && producto == 120 {
	puntosForWhile = puntosForWhile + 1
	print("OK For como while con condición compuesta: correcto")
} else {
	print("X For como while con condición compuesta: incorrecto")
}

print("\nFor como while anidado (patrón X)")
print("\n\n###Validacion Manual")

n := 5
x := 0
for x < n {
	j := 0
	fila := ""

	for j < n {
		if x == j || x+j == n-1 {
			fila = fila + "*"
		} else {
			fila = fila + " "
		}
		j = j + 1
	}

	print(fila)
	x = x + 1
}
print("")

if x == 5 {
	puntosForWhile = puntosForWhile + 2
	print("OK For como while anidado: correcto")
} else {
	print("X For como while anidado: incorrecto")
}

// 4. For Clásico (3 puntos)
print("\n==== For Clásico ====")
puntosForClasico := 0

print("For clásico simple")
suma = 0
for i := 0; i < 5; i++ {
	print("i =", i)
	suma = suma + i
}

if suma == 10 {
	puntosForClasico = puntosForClasico + 1
	print("OK For clásico simple: correcto")
} else {
	print("X For clásico simple: incorrecto")
}

print("\nFor clásico anidado (tabla de multiplicar)")
print("\n\n###Validacion Manual")
for i := 1; i <= 3; i++ {
	for j := 1; j <= 3; j++ {
		print(i, "x", j, "=", i*j)
	}
	print("")
}
print("")

puntosForClasico = puntosForClasico + 2
print("OK For clásico anidado: correcto")

// 5. For Range (3 puntos)
// print("\n==== For Range ====")
// puntosForRange := 0

// print("For range con slice")
// numeros := []int{10, 20, 30, 40, 50}
// suma = 0
// sumaIndices := 0

// for i, valor := range numeros {
// 	print("Índice", i, "=", valor)
// 	suma = suma + valor
// 	sumaIndices = sumaIndices + i
// }

// if suma == 150 {
// 	puntosForRange = puntosForRange + 2
// 	print("OK For range con slice: correcto")
// } else {
// 	print("X For range con slice: incorrecto")
// }

// if sumaIndices == 10 {
// 	puntosForRange = puntosForRange + 1
// 	print("OK For range con índices: correcto")
// } else {
// 	print("X For range con índices: incorrecto")
// }

// 6. Switch/Case (3 puntos)
print("\n==== Switch/Case ====")
puntosSwitch := 0

print("\n\n###Validacion Manual")
print("Switch simple")
dia := 1

switch dia {
case 1:
	print("Lunes")
	puntosSwitch = puntosSwitch + 1
case 2:
	print("Martes")
case 3:
	print("Miércoles")
case 4:
	print("Jueves")
case 5:
	print("Viernes")
case 6:
	print("Sábado")
case 7:
	print("Domingo")
default:
	print("Día inválido")
}

print("\nSwitch con default")
numero := 100

switch numero {
case 1:
	print("No se debería imprimir")
case 2:
	print("No se debería imprimir")
default:
	print("Número no reconocido, se ejecuta default")
	puntosSwitch = puntosSwitch + 1
}

print("\nSwitch con break explícito")

numeroBreak := 2

switch numeroBreak {
case 1:
	print("No se debería imprimir")
case 2:
	print("Caso 2 - Se ejecuta este y debe detenerse")
	puntosSwitch = puntosSwitch + 1
	break
	print("No debería ejecutarse si el break funciona")
	puntosSwitch = puntosSwitch - 1
case 3:
	print("No se debería imprimir")
}
print("")

// 7. Break (3 puntos)
print("\n==== Break ====")
puntosBreak := 0

print("Break en for infinito")
contador := 0
suma = 0

for true {
	print("contador =", contador)
	suma = suma + contador
	contador = contador + 1

	if contador >= 5 {
		break
	}
}

if suma == 10 {
	puntosBreak = puntosBreak + 1
	print("OK Break en for infinito: correcto")
} else {
	print("X Break en for infinito: incorrecto")
}

print("\nBreak en for clásico")
suma = 0

for i := 0; i < 10; i++ {
	print("i =", i)
	suma = suma + i

	if i >= 4 {
		break
	}
}

if suma == 10 {
	puntosBreak = puntosBreak + 2
	print("OK Break en for clásico: correcto")
} else {
	print("X Break en for clásico: incorrecto")
}

// 8. Continue (3 puntos)
print("\n==== Continue ====")
puntosContinue := 0

print("Continue en for tipo while")
contador = 0
impares := 0

for contador < 10 {
	contador = contador + 1

	if contador%2 == 0 {
		continue
	}

	impares = impares + contador
}

print("Números impares:", impares)

if impares == 25 {
	puntosContinue = puntosContinue + 1
	print("OK Continue en for tipo while: correcto")
} else {
	print("X Continue en for tipo while: incorrecto")
}

print("\nContinue en for clásico")
pares := 0

for i := 0; i < 10; i++ {
	if i%2 != 0 {
		continue
	}

	pares = pares + i
}

print("Números pares:", pares)

if pares == 20 {
	puntosContinue = puntosContinue + 2
	print("OK Continue en for clásico: correcto")
} else {
	print("X Continue en for clásico: incorrecto")
}

// Resumen de puntos
puntos = puntosEntornos + puntosIfElse + puntosForWhile + puntosForClasico  + puntosSwitch + puntosBreak + puntosContinue

print("\n=== Tabla de Resultados ===")
print("+--------------------------+--------+-------+")
print("| Característica           | Puntos | Total |")
print("+--------------------------+--------+-------+")
print("| Manejo de entornos       | ", puntosEntornos, "    | 3     |")
print("| If / Else                | ", puntosIfElse, "    | 3     |")
print("| For Tipo While           | ", puntosForWhile, "    | 4     |")
print("| For Clásico              | ", puntosForClasico, "    | 3     |")
//print("| For Range                | ", puntosForRange, "    | 3     |")
print("| Switch/Case              | ", puntosSwitch, "    | 3     |")
print("| Break                    | ", puntosBreak, "    | 3     |")
print("| Continue                 | ", puntosContinue, "    | 3     |")
print("+--------------------------+--------+-------+")
print("| TOTAL                    | ", puntos, "   | 25    |")
print("+--------------------------+--------+-------+")

-------------------------------------------------------------------------------------------

=========================
func ackermann  (m int , n int ) int {
    if (m == 0) {
        return n + 1
    } else if (n == 0) {
        return ackermann(m - 1, 1)
    } else {
        return ackermann(m - 1, ackermann(m, n - 1))
    }
}

print(ackermann(3,4))
======================================
func saludar() {
	print("¡Hola, mundo!")
}

func obtenerNumero() int {
	return 42
}
func saludarPersona(nombre string) {
	print("¡Hola,", nombre+"!")
}

func sumar(a int, b int) int {
	return a + b
}

func factorial(n int) int {
	if n <= 1 {
		return 1
	}
	return n * factorial(n-1)
}

// func ackermann(m int, n int) int {
// 	if m == 0 {
// 		return n + 1
// 	} else if m > 0 && n == 0 {
// 		return ackermann(m-1, 1)
// 	} else {
// 		return ackermann(m-1, ackermann(m, n-1))
// 	}
// }

// // Función recursiva: torres de Hanoi
// func hanoi(n int, origen string, auxiliar string, destino string) {
// 	if n == 1 {
// 		print("Mover disco 1 de", origen, "a", destino)
// 		return
// 	}

// 	hanoi(n-1, origen, destino, auxiliar)
// 	print("Mover disco", n, "de", origen, "a", destino)
// 	hanoi(n-1, auxiliar, origen, destino)
// }

	puntos := 0

	print("=== Archivo de prueba de funciones ===")

	// 1. Funciones no recursivas sin parámetros (5 punto)
	print("==== Funciones no recursivas sin parámetros ====")
	puntosFuncionesSimples := 0

	print("Función sin retorno:")
	print("\n\n###Validacion Manual")
	saludar()

	print("\nFunción con retorno:")
	numero := obtenerNumero()
	print("El número obtenido es:", numero)

	if numero == 42 {
		puntosFuncionesSimples = puntosFuncionesSimples + 5
		print("OK Funciones no recursivas sin parámetros: correcto")
	} else {
		print("X Funciones no recursivas sin parámetros: incorrecto")
	}

	// 2. Funciones no recursivas con parámetros (5 puntos)
	print("\n==== Funciones no recursivas con parámetros ====")
	puntosFuncionesParametros := 0

	print("Función con parámetros sin retorno:")
	print("\n\n###Validacion Manual")
	saludarPersona("Juan")

	print("\nFunción con parámetros y retorno:")
	resultadoSuma := sumar(10, 20)
	print("La suma de 10 y 20 es:", resultadoSuma)

	if resultadoSuma == 30 {
		puntosFuncionesParametros = puntosFuncionesParametros + 5
		print("OK Función con parámetros y retorno: correcto")
	} else {
		print("X Función con parámetros y retorno: incorrecto")
	}

	// 3. Funciones recursivas (10 puntos)
	print("\n==== Funciones recursivas ====")
	puntosFuncionesRecursivas := 0

	print("Factorial:")
	resultadoFactorial := factorial(5)
	print("Factorial de 5:", resultadoFactorial)

	if resultadoFactorial == 120 {
		puntosFuncionesRecursivas = puntosFuncionesRecursivas + 3
		print("OK Función factorial: correcto")
	} else {
		print("X Función factorial: incorrecto")
	}

	// print("\nTorres de Hanoi:")
	// print("\n\n###Validacion Manual")
	// hanoi(3, "A", "B", "C")
	// puntosFuncionesRecursivas = puntosFuncionesRecursivas + 3

	print("\nAckermann:")
	// resultadoAckermann := ackermann(3, 2)
	// print("Ackermann de (3, 2):", resultadoAckermann)

	// if resultadoAckermann == 29 {
	// 	puntosFuncionesRecursivas = puntosFuncionesRecursivas + 4
	// 	print("OK Función ackermann: correcto")
	// } else {
	// 	print("X Función ackermann: incorrecto")
	// }

	// 4. strconv.Atoi (2 punto)
	print("\n==== strconv.Atoi ====")
	puntosAtoi := 0


	puntos = puntosFuncionesSimples + puntosFuncionesParametros + puntosFuncionesRecursivas 
		 

	print("\n=== Tabla de Resultados ===")
	print("+----------------------------------+--------+-------+")
	print("| Característica                   | Puntos | Total |")
	print("+----------------------------------+--------+-------+")
	print("| Funciones sin parámetros         | ", puntosFuncionesSimples, "    | 5     |")
	print("| Funciones con parámetros         | ", puntosFuncionesParametros, "    | 5     |")
	print("| Funciones recursivas             | ", puntosFuncionesRecursivas, "   | 10    |")
	print("+----------------------------------+--------+-------+")
	print("| TOTAL                            | ", puntos, "   | 25    |")
	print("+----------------------------------+--------+-------+")