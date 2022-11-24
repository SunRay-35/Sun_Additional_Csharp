// Задача 4. (*) Напишите программу, которая заполнит спирально массив 4 на 4.
// Например, на выходе получается вот такой массив:
// 01 02 03 04
// 12 13 14 05
// 11 16 15 06
// 10 09 08 07

// Решение не работает при m % 2 == 0 && n % 2 != 0 и работает до массива 8х8. Сложно


int ReadData(string message)
{
    Console.WriteLine(message);
    return Convert.ToInt32(Console.ReadLine());
}

void PrintArray(int[,] array)
{
    for (int i = 0; i < array.GetLength(0); i++)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
            Console.Write($"{array[i, j]}\t");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

int MoveRight(int[,] array, int i, int j, int step)
{
    if (step == array.GetLength(0) * array.GetLength(1))
    {
        array[i, j] = step;
        step++;
        return step;
    }
    int currentWidth = array.GetLength(1) - j - 1;
    for (int n = j; n < currentWidth; n++) // чтобы не сбить стартовую позицию
    {
        array[i, n] = step;
        // Console.WriteLine($"{i},{n}");
        step++;
    }
    step = MoveDown(array, i, j, step);
    return step;
}

int MoveDown(int[,] array, int i, int j, int step)
{
    int currentWidth = array.GetLength(1) - j - 1;
    int currentHeight = array.GetLength(0) - i - 1;
    for (int n = i; n < currentHeight; n++) // чтобы не сбить стартовую позицию
    {
        array[n, currentWidth] = step;
        // Console.WriteLine($"{n},{currentWidth}");
        step++;
    }
    step = MoveLeft(array, i, j, step);
    return step;
}

int MoveLeft(int[,] array, int i, int j, int step)
{
    int currentWidth = array.GetLength(1) - j - 1;
    int currentHeight = array.GetLength(0) - i - 1;
    for (int n = currentWidth; n > j; n--) // чтобы не сбить стартовую позицию
    {
        array[currentHeight, n] = step;
        // Console.WriteLine($"{currentHeight},{n}");
        step++;
    }
    step = MoveUp(array, i, j, step);
    return step;
}

int MoveUp(int[,] array, int i, int j, int step)
{
    int currentHeight = array.GetLength(0) - i - 1;
    for (int n = currentHeight; n > i; n--) // чтобы не сбить стартовую позицию
    {
        array[n, j] = step;
        // Console.WriteLine($"{n},{j}");
        step++;
    }
    return step;
}

void StepRoundArray(int[,] array, int i, int j, int step)
{
    // int k = i; //Start position
    // int l = j; //Start position
    // int number;
    // if (step <= array.GetLength(0) / 2)
    // {
    //     if (step % 2 == 0 || array.GetLength(0) > 6) number = array[k + step, l + ((int)(Math.Round((double)step / 2)))] + 1; // Ужасная формула, понимаю, получил "тупым" подбором. Просто прибавлять step- не работает почему-то
    //     else number = array[k + step, l] + 1;
    //     MoveRight(array, k + step, l + step, number, step);
    //     StepRoundArray(array, k, l, (step + 1));
    // }
    if (step <= array.GetLength(0) * array.GetLength(1))
    {
        // Console.WriteLine();
        step = MoveRight(array, i, j, step);
        i++;
        j++;
        StepRoundArray(array, i, j, step);
    }
}

void Main()
{
    int m = ReadData("Input an amount of the rows in array");
    int n = ReadData("Input an amount of the columns in array");
    int i = 0; // Start position index1
    int j = 0; // Start position index2
    int step = 1; // A necessary variable for recurtion
    int[,] array = new int[m, n];
    StepRoundArray(array, i, j, step);
    PrintArray(array);
}

Main();