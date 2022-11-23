using System;
using System.Drawing;

void SetConsoleOptions() // Подготовка консоли
{
    System.Console.Clear();
    System.Console.BackgroundColor = System.ConsoleColor.Black;
}

System.ConsoleColor GetConsoleColor(System.Drawing.Color pixelColor) // Ответ 24: https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
{
    int index = (pixelColor.R > 128 | pixelColor.G > 128 | pixelColor.B > 128) ? 8 : 0; // Bright bit
    index |= (pixelColor.R > 64) ? 4 : 0; // Red bit
    index |= (pixelColor.G > 64) ? 2 : 0; // Green bit
    index |= (pixelColor.B > 64) ? 1 : 0; // Blue bit
    return (System.ConsoleColor)index;
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

Bitmap GetResizedImage (Bitmap imageSource) // Масштабируем изображения под размеры консоли
{
    var consoleParams = GetConsoleSize();
    double imageScale = consoleParams.height * 1.0 / imageSource.Height; // Масштаб изображения с учетом размеров консоли
    var newImageSize = GetNewSize(imageSource.Width, imageSource.Height, imageScale);
    return new Bitmap(imageSource, newImageSize.width, newImageSize.height);
}

void DrawBitmap(Bitmap imageSource) // Выводим рисунок в консоль с помощью цвета
{
    Bitmap imageScaled = GetResizedImage (imageSource);
    for (int j = 0; j < imageScaled.Height; j++)
    {
        for (int i = 0; i < imageScaled.Width; i++)
        {
            System.Console.ForegroundColor = GetConsoleColor(imageScaled.GetPixel(i, j)); // Обратный порядок, так как используются координаты x и y
            System.Console.Write("██"); // Символ █ (2588) взял с https://ru.wikipedia.org/wiki/Псевдографика
        }
        System.Console.WriteLine();
    }
}

void DrawSymbols(Bitmap imageSource) // Выводим рисунок в консоль с помощью символов
{
    Bitmap imageScaled = GetResizedImage (imageSource);
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
    if (PromptMode("Выберите режим рисования( 1 - символами, 2 - цветами): "))
    {
        DrawBitmap(imageSource); // Для 2 - цветом
    }
    else
    {
        DrawSymbols(imageSource); // Для 1 - символами
    }
}

Main();