.data
heap: .space 4096
.text
.global _start
_start:
    adr x10, heap
// Print statement
// visiting expression
// String constant: \n\n###Validacion Manual
STR x10, [SP, #-8]!
// Pushing character 92 to heap
MOV w0, #92
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 92 to heap
MOV w0, #92
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 35 to heap
MOV w0, #35
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 35 to heap
MOV w0, #35
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 35 to heap
MOV w0, #35
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 86 to heap
MOV w0, #86
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 77 to heap
MOV w0, #77
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Print statement
// visiting expression
// String constant: Switch simple
STR x10, [SP, #-8]!
// Pushing character 83 to heap
MOV w0, #83
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 119 to heap
MOV w0, #119
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 104 to heap
MOV w0, #104
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 112 to heap
MOV w0, #112
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Variable declaration: dia
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
// Variable dia registered in stack
// Variable declaration: puntosSwitch
// Constant: 0
MOV x0, #0
STR x0, [SP, #-8]!
// Variable puntosSwitch registered in stack
// Switch statement
MOV x0, #8
ADD x0, sp, x0
LDR x0, [x0, #0]
STR x0, [SP, #-8]!
LDR x0, [SP], #8
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L1
// Constant: 2
MOV x0, #2
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L2
// Constant: 3
MOV x0, #3
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L3
// Constant: 4
MOV x0, #4
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L4
// Constant: 5
MOV x0, #5
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L5
// Constant: 6
MOV x0, #6
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L6
// Constant: 7
MOV x0, #7
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L7
B L8
L1:
// Print statement
// visiting expression
// String constant: Lunes
STR x10, [SP, #-8]!
// Pushing character 76 to heap
MOV w0, #76
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Expression instruction
// Assignment to variable: puntosSwitch
// Operación de suma o resta
MOV x0, #0
ADD x0, sp, x0
LDR x0, [x0, #0]
STR x0, [SP, #-8]!
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
LDR x0, [SP], #8
LDR x1, [SP], #8
// Integer addition
ADD x0, x1, x0
STR x0, [SP, #-8]!
LDR x0, [SP], #8
MOV x1, #0
ADD x1, sp, x1
STR x0, [x1, #0]
STR x0, [SP, #-8]!
// Popping value to discard
LDR x0, [SP], #8
B L0
L2:
// Print statement
// visiting expression
// String constant: Martes
STR x10, [SP, #-8]!
// Pushing character 77 to heap
MOV w0, #77
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L0
L3:
// Print statement
// visiting expression
// String constant: Miércoles
STR x10, [SP, #-8]!
// Pushing character 77 to heap
MOV w0, #77
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 233 to heap
MOV w0, #233
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L0
L4:
// Print statement
// visiting expression
// String constant: Jueves
STR x10, [SP, #-8]!
// Pushing character 74 to heap
MOV w0, #74
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 118 to heap
MOV w0, #118
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L0
L5:
// Print statement
// visiting expression
// String constant: Viernes
STR x10, [SP, #-8]!
// Pushing character 86 to heap
MOV w0, #86
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L0
L6:
// Print statement
// visiting expression
// String constant: Sábado
STR x10, [SP, #-8]!
// Pushing character 83 to heap
MOV w0, #83
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 225 to heap
MOV w0, #225
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L0
L7:
// Print statement
// visiting expression
// String constant: Domingo
STR x10, [SP, #-8]!
// Pushing character 68 to heap
MOV w0, #68
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 103 to heap
MOV w0, #103
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L0
L8:
// Print statement
// visiting expression
// String constant: Día inválido
STR x10, [SP, #-8]!
// Pushing character 68 to heap
MOV w0, #68
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 237 to heap
MOV w0, #237
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 118 to heap
MOV w0, #118
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 225 to heap
MOV w0, #225
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
L0:
// End of switch statement
// Print statement
// visiting expression
// String constant: \nSwitch con default
STR x10, [SP, #-8]!
// Pushing character 92 to heap
MOV w0, #92
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 83 to heap
MOV w0, #83
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 119 to heap
MOV w0, #119
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 104 to heap
MOV w0, #104
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 102 to heap
MOV w0, #102
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Variable declaration: numero
// Constant: 100
MOV x0, #100
STR x0, [SP, #-8]!
// Variable numero registered in stack
// Switch statement
MOV x0, #0
ADD x0, sp, x0
LDR x0, [x0, #0]
STR x0, [SP, #-8]!
LDR x0, [SP], #8
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L10
// Constant: 2
MOV x0, #2
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L11
B L12
L10:
// Print statement
// visiting expression
// String constant: No se debería imprimir
STR x10, [SP, #-8]!
// Pushing character 78 to heap
MOV w0, #78
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 237 to heap
MOV w0, #237
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 112 to heap
MOV w0, #112
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L9
L11:
// Print statement
// visiting expression
// String constant: No se debería imprimir
STR x10, [SP, #-8]!
// Pushing character 78 to heap
MOV w0, #78
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 237 to heap
MOV w0, #237
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 112 to heap
MOV w0, #112
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L9
L12:
// Print statement
// visiting expression
// String constant: Número no reconocido, se ejecuta default
STR x10, [SP, #-8]!
// Pushing character 78 to heap
MOV w0, #78
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 250 to heap
MOV w0, #250
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 44 to heap
MOV w0, #44
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 106 to heap
MOV w0, #106
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 102 to heap
MOV w0, #102
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Expression instruction
// Assignment to variable: puntosSwitch
// Operación de suma o resta
MOV x0, #8
ADD x0, sp, x0
LDR x0, [x0, #0]
STR x0, [SP, #-8]!
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
LDR x0, [SP], #8
LDR x1, [SP], #8
// Integer addition
ADD x0, x1, x0
STR x0, [SP, #-8]!
LDR x0, [SP], #8
MOV x1, #8
ADD x1, sp, x1
STR x0, [x1, #0]
STR x0, [SP, #-8]!
// Popping value to discard
LDR x0, [SP], #8
L9:
// End of switch statement
// Print statement
// visiting expression
// String constant: \nSwitch con break explícito
STR x10, [SP, #-8]!
// Pushing character 92 to heap
MOV w0, #92
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 83 to heap
MOV w0, #83
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 119 to heap
MOV w0, #119
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 104 to heap
MOV w0, #104
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 107 to heap
MOV w0, #107
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 120 to heap
MOV w0, #120
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 112 to heap
MOV w0, #112
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 237 to heap
MOV w0, #237
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Variable declaration: numeroBreak
// Constant: 2
MOV x0, #2
STR x0, [SP, #-8]!
// Variable numeroBreak registered in stack
// Switch statement
MOV x0, #0
ADD x0, sp, x0
LDR x0, [x0, #0]
STR x0, [SP, #-8]!
LDR x0, [SP], #8
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L14
// Constant: 2
MOV x0, #2
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L15
// Constant: 3
MOV x0, #3
STR x0, [SP, #-8]!
LDR x1, [SP], #8
CMP x0, x1
BEQ L16
L14:
// Print statement
// visiting expression
// String constant: No se debería imprimir
STR x10, [SP, #-8]!
// Pushing character 78 to heap
MOV w0, #78
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 237 to heap
MOV w0, #237
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 112 to heap
MOV w0, #112
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L13
L15:
// Print statement
// visiting expression
// String constant: Caso 2 - Se ejecuta este y debe detenerse
STR x10, [SP, #-8]!
// Pushing character 67 to heap
MOV w0, #67
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 50 to heap
MOV w0, #50
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 45 to heap
MOV w0, #45
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 83 to heap
MOV w0, #83
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 106 to heap
MOV w0, #106
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 121 to heap
MOV w0, #121
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Expression instruction
// Assignment to variable: puntosSwitch
// Operación de suma o resta
MOV x0, #16
ADD x0, sp, x0
LDR x0, [x0, #0]
STR x0, [SP, #-8]!
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
LDR x0, [SP], #8
LDR x1, [SP], #8
// Integer addition
ADD x0, x1, x0
STR x0, [SP, #-8]!
LDR x0, [SP], #8
MOV x1, #16
ADD x1, sp, x1
STR x0, [x1, #0]
STR x0, [SP, #-8]!
// Popping value to discard
LDR x0, [SP], #8
// Break statement
B L13
// Print statement
// visiting expression
// String constant: No debería ejecutarse si el break funciona
STR x10, [SP, #-8]!
// Pushing character 78 to heap
MOV w0, #78
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 237 to heap
MOV w0, #237
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 106 to heap
MOV w0, #106
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 116 to heap
MOV w0, #116
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 108 to heap
MOV w0, #108
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 107 to heap
MOV w0, #107
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 102 to heap
MOV w0, #102
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 117 to heap
MOV w0, #117
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 99 to heap
MOV w0, #99
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 110 to heap
MOV w0, #110
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
// Expression instruction
// Assignment to variable: puntosSwitch
// Operación de suma o resta
MOV x0, #16
ADD x0, sp, x0
LDR x0, [x0, #0]
STR x0, [SP, #-8]!
// Constant: 1
MOV x0, #1
STR x0, [SP, #-8]!
LDR x0, [SP], #8
LDR x1, [SP], #8
// Integer subtraction
SUB x0, x1, x0
STR x0, [SP, #-8]!
LDR x0, [SP], #8
MOV x1, #16
ADD x1, sp, x1
STR x0, [x1, #0]
STR x0, [SP, #-8]!
// Popping value to discard
LDR x0, [SP], #8
B L13
L16:
// Print statement
// visiting expression
// String constant: No se debería imprimir
STR x10, [SP, #-8]!
// Pushing character 78 to heap
MOV w0, #78
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 111 to heap
MOV w0, #111
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 115 to heap
MOV w0, #115
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 100 to heap
MOV w0, #100
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 98 to heap
MOV w0, #98
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 101 to heap
MOV w0, #101
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 237 to heap
MOV w0, #237
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 97 to heap
MOV w0, #97
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 32 to heap
MOV w0, #32
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 112 to heap
MOV w0, #112
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 109 to heap
MOV w0, #109
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 105 to heap
MOV w0, #105
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 114 to heap
MOV w0, #114
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
B L13
L13:
// End of switch statement
// Print statement
// visiting expression
// String constant: 
STR x10, [SP, #-8]!
// Pushing character 0 to heap
MOV w0, #0
STRB w0, [x10]
MOV x0, #1
ADD x10, x10, x0
// Popping value to print
LDR x0, [SP], #8
MOV X0, x0
BL print_string
bl print_newline
MOV x0, #0
MOV x8, #93
SVC #0



 // Foreign functions



 // Standard Library Functions

//--------------------------------------------------------------
// print_string - Prints a null-terminated string to stdout
//
// Input:
//   x0 - The address of the null-terminated string to print
//--------------------------------------------------------------
print_string:
    // Save link register and other registers we'll use
    stp     x29, x30, [sp, #-16]!
    stp     x19, x20, [sp, #-16]!
    
    // x19 will hold the string address
    mov     x19, x0
    
print_loop:
    // Load a byte from the string
    ldrb    w20, [x19]
    
    // Check if it's the null terminator (0)
    cbz     w20, print_done
    
    // Prepare for write syscall
    mov     x0, #1              // File descriptor: 1 for stdout
    mov     x1, x19             // Address of the character to print
    mov     x2, #1              // Length: 1 byte
    mov     x8, #64             // syscall: write (64 on ARM64)
    svc     #0                  // Make the syscall
    
    // Move to the next character
    add     x19, x19, #1
    
    // Continue the loop
    b       print_loop
    
print_done:
    // Restore saved registers
    ldp     x19, x20, [sp], #16
    ldp     x29, x30, [sp], #16
    ret
    // Return to the caller
    

    print_newline:
        mov x0, #1
        adr x1, newline_char
        mov x2, #1
        mov w8, #64
        svc #0
        ret
    
newline_char: .ascii "\n"