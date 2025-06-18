#include <iostream>
using namespace std;

int calculateY(int x, int a)
{
    int y;
    __asm {
        mov eax, x
        cmp eax, -10
        jl case1
        cmp eax, 10
        jl case2

        case3:
        mov eax, a
        sub eax, x
        jmp finish
        
        case2:
        mov eax, x
        test eax, eax
        jns abs_done
        neg eax
        abs_done:
        imul eax, a
        jmp finish

        case1:
        imul eax, x
        imul eax, a
        finish:

        mov y, eax
    }
    return y;
}

int main()
{
    setlocale(LC_ALL, "Russian");
    int x, a;
    cout << "Введите x и a: ";
    cin >> x >> a;

    cout << "Значение y: " << calculateY(x, a) << endl;
    return 0;
}
