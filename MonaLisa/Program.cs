using System;
using System.Drawing;

void SetConsoleOptions() // Подготовка консоли
{
    System.Console.Clear();
    System.Console.BackgroundColor = System.ConsoleColor.Black;
}

// System.ConsoleColor GetConsoleColor(System.Drawing.Color pixelColor) // Ответ 24: https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
// {
//     int index = (pixelColor.R > 128 | pixelColor.G > 128 | pixelColor.B > 128) ? 8 : 0; // Bright bit
//     index |= (pixelColor.R > 64) ? 4 : 0; // Red bit
//     index |= (pixelColor.G > 64) ? 2 : 0; // Green bit
//     index |= (pixelColor.B > 64) ? 1 : 0; // Blue bit
//     return (System.ConsoleColor)index;
// }

double DistanceRGB(int r1, int g1, int b1, int r2, int g2, int b2) // Находим расстояние между цветами
{
    return Math.Sqrt(Math.Pow(r2 - r1, 2) + Math.Pow(g2 - g1, 2) + Math.Pow(b2 - b1, 2));
}

System.ConsoleColor GetConsoleColor(System.Drawing.Color pixelColor) // Мой вариант метода определения цвета на основании расстояний между цветами
// 0 Black        0, 0, 0
// 1 DarkBlue     4, 81, 165
// 2 DarkGreen    0, 188, 0
// 3 DarkCyan     5, 152, 188
// 4 DarkRed      205, 49, 49
// 5 DarkMagenta  188, 5, 188
// 6 DarkYellow   148, 152, 0
// 7 Gray         51, 51, 51
// 8 DarkGray     102, 102, 102
// 9 Blue         4, 81, 165
// 10 Green        20, 206, 20
// 11 Cyan         5, 152, 188
// 12 Red          205, 49, 49
// 13 Magenta      188, 5, 188
// 14 Yellow       181, 186, 0
// 15 White        165, 165, 165 - больше тянет на светло-серый
{
    // Оттенки цветов убрал, мы из будем регулировать интенсивностью символа.
    int[,] palette = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 255 }, { 0, 255, 0 }, { 0, 255, 255 }, { 255, 0, 0 }, { 255, 0, 255 }, { 255, 255, 0 }, { 0, 0, 0 } };
    double minDistance = 500;
    int minColor = 0;
    for (int k = 0; k < palette.GetLength(0); k++)
    {
        double temp = DistanceRGB(pixelColor.R, pixelColor.G, pixelColor.B, palette[k, 0], palette[k, 1], palette[k, 2]);
        if (temp < minDistance)
        {
            minDistance = temp;
            minColor = k;
        }
    }
    double brightness = (0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B);
    if (brightness < 128)
    {
        if (minColor == 9) minColor = 1;
        if (minColor == 10) minColor = 2;
        if (minColor == 11) minColor = 3;
        if (minColor == 12) minColor = 4;
        if (minColor == 13) minColor = 5;
        if (minColor == 14) minColor = 5;
        if (minColor == 7) minColor = 8;
    }
    return (System.ConsoleColor)minColor;
}

string GetSymbol(System.Drawing.Color pixelColor) // Подбираем символ на основе яркости. Символы взял с https://ru.wikipedia.org/wiki/Псевдографика
{
    double brightness = (0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B); // Формулу яркости взял в ответе 456 https://qastack.ru/programming/596216/formula-to-determine-brightness-of-rgb-color
    if (brightness >= 0 && brightness < 51) return "██";
    if (brightness >= 51 && brightness < 102) return "▓▓";
    if (brightness >= 102 && brightness < 153) return "▒▒";
    if (brightness >= 153 && brightness < 204) return "░░";
    return "  ";
}

(int width, int height) GetConsoleSize() // Определяем доступный размер консоли
{
    int tempWidth = System.Console.WindowWidth;
    int tempHeight = System.Console.WindowHeight;
    return (tempWidth, tempHeight);
}

(int width, int height) GetNewSize(int width, int height, double scale) // Изменение заданных размеров с учетом коэффициента
{
    int tempWidth = Convert.ToInt32(width * scale);
    int tempHeight = Convert.ToInt32(height * scale);
    return (tempWidth, tempHeight);
}

Bitmap GetResizedImage(Bitmap imageSource) // Масштабируем изображения под размеры консоли
{
    var consoleParams = GetConsoleSize();
    double imageScale = consoleParams.height * 1.0 / imageSource.Height; // Масштаб изображения с учетом размеров консоли
    var newImageSize = GetNewSize(imageSource.Width, imageSource.Height, imageScale);
    return new Bitmap(imageSource, newImageSize.width, newImageSize.height);
}

void DrawBitmap(Bitmap imageSource) // Выводим рисунок в консоль с помощью цвета
{
    Bitmap imageScaled = GetResizedImage(imageSource);
    for (int j = 0; j < imageScaled.Height; j++)
    {
        for (int i = 0; i < imageScaled.Width; i++)
        {
            System.Console.ForegroundColor = GetConsoleColor(imageScaled.GetPixel(i, j)); // Обратный порядок, так как используются координаты x и y
            System.Console.Write("██");
            // System.Console.Write(GetSymbol(imageScaled.GetPixel(i, j)));
        }
        System.Console.WriteLine();
    }
}

void DrawSymbols(Bitmap imageSource) // Выводим рисунок в консоль с помощью символов
{
    Bitmap imageScaled = GetResizedImage(imageSource);
    for (int j = 0; j < imageScaled.Height; j++)
    {
        for (int i = 0; i < imageScaled.Width; i++)
        {
            System.Console.Write(GetSymbol(imageScaled.GetPixel(i, j))); // Обратный порядок, так как используются координаты x и y
        }
        System.Console.WriteLine();
    }
}

bool PromptMode(string message) // Специфичный запрос для выбора режима рисования
{
    System.Console.Write(message);
    int temp = Convert.ToInt32(System.Console.ReadLine());
    if (temp != 2) return false;
    else return true;
}

void Main() // Основное тело программы
{
    SetConsoleOptions();
    Bitmap imageSource = new Bitmap(@"image.bmp", true); // Файл лежит в папке с проектом под именем image.bmp
    DrawBitmap(imageSource);
}

Main();