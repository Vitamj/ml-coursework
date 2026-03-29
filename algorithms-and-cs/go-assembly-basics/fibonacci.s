#include "textflag.h"

TEXT ·Fibonacci(SB), NOSPLIT, $0-16
	MOVD	n+0(FP), R0
	MOVD	$0, R1
	MOVD	$1, R2

fib_loop:
	CBZ	R0, fib_done
	ADD	R1, R2, R3
	SUB	$1, R0, R0
	MOVD	R2, R1
	MOVD	R3, R2
	B	fib_loop

fib_done:
	MOVD	R1, ret+8(FP)
	RET