#include <iostream>

using namespace std;

int getDaysInMonth(int month) {
    int days;
    int monthDays[] = { 31,28,31,30,31,30,31,31,30,31,30,31 };

    __asm {
        mov eax, month
        dec eax
        lea ebx, monthDays
        mov eax, [ebx + eax * 4]
        mov days, eax
    }
    return days;
}

int main() {
    setlocale(LC_ALL, "Russian");
    int month;
    cout << "Введите номер месяца: ";
    cin >> month;

    if (month < 1 || month > 12) {
        cout << "Некорректный номер месяца!" << endl;
    }
    else {
        cout << "Количество дней: " << getDaysInMonth(month) << endl;
    }
    return 0;
}

