#include "textflag.h"

TEXT ·LowerBound(SB), NOSPLIT, $0-40
	MOVD	slice_base+0(FP), R0
	MOVD	slice_len+8(FP), R1
	MOVD	value+24(FP), R2

	MOVD	$0, R3
	MOVD	R1, R4

lb_loop:
	CMP	R3, R4
	BEQ	lb_done

	ADD	R3, R4, R5
	LSR	$1, R5, R5

	LSL	$3, R5, R6
	ADD	R0, R6, R6
	MOVD	(R6), R7

	CMP	R2, R7
	BLT	lb_move_low

	MOVD	R5, R4
	B	lb_loop

lb_move_low:
	ADD	$1, R5, R3
	B	lb_loop

lb_done:
	MOVD	R3, ret+32(FP)
	RET