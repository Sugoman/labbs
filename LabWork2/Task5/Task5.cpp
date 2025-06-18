#include <iostream>

using namespace std;

void calculateChange(int price, int paid) {
    int change;
    __asm {
        mov eax, paid
        sub eax, price
        jg give_change
        jl not_enough

        exact_amount :
        call printf
        add esp, 4
        jmp finish

        give_change:
        mov change, eax
        push change
        add esp, 8
        jmp finish

        not_enough:
        neg eax
        mov change, eax
        push change
        call printf
        add esp, 8

    finish:
    }
}

int main() {
    setlocale(LC_ALL, "Russian");
    int price, paid;
    cout << "Введите сумму покупки и внесенную сумму: ";
    cin >> price >> paid;

    if (paid == price)
    {
        cout << "Спасибо!" << endl;
    }
    else if (paid > price)
    {
        cout << "Возьмите сдачу: " << (paid - price) << endl;
    }
    else
    {
        cout << "Недостаточно средств, нужно ещё: " << (price - paid) << endl;
    }

    return 0;
}
