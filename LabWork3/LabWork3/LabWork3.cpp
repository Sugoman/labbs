#include <iostream>

void Task_5_1()
{
    int N;
    std::cout << "Введите число N: ";
    std::cin >> N;

    if (N < 0) {
        std::cout << "Факториал не существует" << std::endl;
        return;
    }

    int factorial = 1;
    __asm {
        mov eax, 1
        mov ecx, N
        test ecx, ecx
        jz end_factorial
        factorial_loop :
        mul ecx
            loop factorial_loop
            end_factorial :
        mov factorial, eax
    }
    std::cout << "Факториал: " << factorial << std::endl;
}

void Task_5_2()
{
    int N, count = 0;
    std::cout << "Введите число N: ";
    std::cin >> N;

    if (N <= 0) {
        std::cout << "Число должно быть положительным!" << std::endl;
        return;
    }

    __asm {
        mov ecx, 1
        mov ebx, N
        mov edi, 0
    count_loop:
        cmp ecx, ebx
        jg end_count
        mov eax, ebx
        cdq
        div ecx
        test edx, edx
        jnz skip_increment
        inc edi
    skip_increment :
        inc ecx
        jmp count_loop
    end_count:
        mov count, edi
    }
    std::cout << "Количество делителей: " << count << std::endl;
}

void print_value(int value) {
    std::cout << value << " ";
}

void Task_5_3()
{
    int N;
    std::cout << "Введите число N: ";
    std::cin >> N;

    std::cout << "Числа, кратные 5: ";
    __asm {
        mov eax, N
        cdq
        mov ecx, 5
        div ecx
        mul ecx
        mov ecx, eax
    print_loop :
        cmp ecx, 0
        jle end_print
        push ecx
        call print_value
        pop ecx
        sub ecx, 5
        jmp print_loop
    end_print :
    }
    std::cout << std::endl;
}



void Task_5_4()
{
    std::cout << "Таблица умножения:" << std::endl;
    for (int i = 1; i <= 10; i++) {
        for (int j = 1; j <= 10; j++) {
            int result;
            __asm {
                mov eax, i
                mul j
                mov result, eax
            }
            std::cout << result << "\t";
        }
        std::cout << std::endl;
    }
}

int main()
{
    setlocale(LC_ALL, "Russian");
    Task_5_1();
    Task_5_2();
    Task_5_3();
    Task_5_4();
    return 0;
}