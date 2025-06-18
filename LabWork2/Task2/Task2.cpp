#include <iostream>
using namespace std;

int findMax(int a, int b, int c)
{
    int result;
    __asm {
        mov eax, a
        mov ebx, b
        cmp eax, ebx
        jge first_is_bigger
        mov eax, ebx
        first_is_bigger:
        mov ebx, c
        cmp eax, ebx
        jge second_is_bigger
        mov eax, ebx
        second_is_bigger:
        mov result, eax
    }
    return result;
}

int main()
{
    setlocale(LC_ALL, "Russian");
    int a, b, c;
    cout << "Введите три числа: ";
    cin >> a >> b >> c;

    cout << "Максимальное число: " << findMax(a, b, c) << endl;
    return 0;
}