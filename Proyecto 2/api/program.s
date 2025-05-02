.data
heap: .space 4096
.text
.global _start
_start:
    adr x10, heap
    // Call to main
    BL main
    MOV x0, #0
    MOV x8, #93
    SVC #0
// End of function main
// Call to main
BL main



 // Foreign functions
main:
// Prologue de main
STP x29, x30, [SP, #-16]!
ADD x29, sp, xzr
// Print statement
// visiting expression
// String constant: hola
STR x10, [SP, #-8]!
// Pushing character 104 to heap
MOV w0, #104
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
L0:
// Epilogue de main
LDP x29, x30, [sp, #0]
RET
// Function main frame size: 3 bytes



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