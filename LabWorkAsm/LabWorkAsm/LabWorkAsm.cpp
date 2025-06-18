#include <iostream>

using namespace std;

int main()
{
    _asm {
        // цикл с пред условиекм

    while_begin:
        cmp EAX, ECX;
        jl end_while

            ; body

           jmp while_begin
    end_while

            // цикл с под условием
            dowhile_begin :
        cmp EAX, ECX;
        jg end_while

            ; body

            jg end_while
            jmp while_begin
            end_dowhile

            // цикл с счётчиком
    mov ECX, 0
            for_begin:
        cmp ECX, 5
            jge end_for

            ; body
            inc ECX
            jmp for_begin
        end_for:

    }
}
