#include <iostream>

using namespace std;

int findMin(int a, int b)
{
    int result;
    __asm {
        mov eax, a
        mov ebx, b
        cmp eax, ebx
        jle less_or_equal
        mov eax, ebx
        less_or_equal:
        mov result, eax
    }
    return result;
}

int main()
{
    setlocale(LC_ALL, "Russian");
    int a, b;
    cout << "Введите два числа: ";
    cin >> a >> b;

    cout << "Минимальное число: " << findMin(a,b)<<endl;
    return 0;
}

