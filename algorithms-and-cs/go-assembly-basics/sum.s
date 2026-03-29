#include "textflag.h"

TEXT ·Sum(SB), NOSPLIT, $0-32
        MOVD    x_base+0(FP), R0
        MOVD    x_len+8(FP), R1
        MOVD    $0, R2

sum_loop:
        CBZ     R1, sum_done
        MOVW    (R0), R3
        SXTW    R3, R3
        ADD     R3, R2, R2
        ADD     $4, R0, R0
        SUB     $1, R1, R1
        B       sum_loop

sum_done:
        MOVD    R2, ret+24(FP)
        RET