using System;
using System.Threading;

class GameThreads
{
    public string snakeDirection;
    public int gameWidth;
    public int gameHeight;
    public int offsetTop;
    public int offsetInfo;
    public void SnakeActions() // Действия змейки
    {
        int delayDuration = 500;
        double incTail = 0.00000000000001;
        double[,] gameField = new double[gameHeight, gameWidth];
        for (int j = 0; j < gameWidth; j++)
        {
            gameField[offsetTop, j] = 1;
            gameField[gameHeight - 1, j] = 1;
        }
        for (int i = 1; i < gameHeight - 1; i++)
        {
            gameField[i, 0] = 1;
            gameField[i, gameWidth - 1] = 1;
        }
        bool IsAlive = true;
        int headHeight = Convert.ToInt32(gameHeight / 2);
        int headWidth = Convert.ToInt32(gameWidth / 2);
        gameField[headHeight, headWidth] = 1;
        int tailHeight = headHeight - 1;
        int tailWidth = headWidth;
        double tailAge = 2.0;
        gameField[tailHeight, tailWidth] = tailAge;
        // System.Console.SetCursorPosition(headWidth, headHeight);
        // System.Console.Write("*");
        // System.Console.SetCursorPosition(tailWidth, tailHeight);
        // System.Console.Write("*");
        while (IsAlive)
        {
            gameField[headHeight, headWidth] = 3;
            if (snakeDirection == "up" && (gameField[headHeight - 1, headWidth] != 1))
            {
                gameField[headHeight - 1, headWidth] = 2;
                headHeight--;
                System.Console.SetCursorPosition(headHeight, headWidth);
                System.Console.Write("*");
                System.Console.SetCursorPosition(tailWidth, tailHeight);
                System.Console.Write(" ");
                gameField[tailHeight, tailWidth] = 0;
            }
            else if (snakeDirection == "down" && (gameField[headHeight + 1, headWidth] != 1))
            {
                gameField[headHeight + 1, headWidth] = 2;
                headHeight++;
                System.Console.SetCursorPosition(headHeight, headWidth);
                System.Console.Write("*");
                System.Console.SetCursorPosition(tailWidth, tailHeight);
                System.Console.Write(" ");
                gameField[tailHeight, tailWidth] = 0;
            }
            else if (snakeDirection == "right" && (gameField[headHeight, headWidth + 1] != 1))
            {
                gameField[headHeight, headWidth + 1] = 2;
                headWidth++;
                System.Console.SetCursorPosition(headHeight, headWidth);
                System.Console.Write("*");
                System.Console.SetCursorPosition(tailWidth, tailHeight);
                System.Console.Write(" ");
                gameField[tailHeight, tailWidth] = 0;
            }
            else if (snakeDirection == "left" && (gameField[headHeight, headWidth - 1] != 1))
            {
                gameField[headHeight, headWidth - 1] = 2;
                headWidth--;
                System.Console.SetCursorPosition(headHeight, headWidth);
                System.Console.Write("*");
                System.Console.SetCursorPosition(tailWidth, tailHeight);
                System.Console.Write(" ");
                gameField[tailHeight, tailWidth] = 0;
            }
            else IsAlive = false;
            if (gameField[tailHeight - 1, tailWidth] == tailAge) tailHeight--;
            else if (gameField[tailHeight + 1, tailWidth] == tailAge) tailHeight++;
            else if (gameField[tailHeight, tailWidth - 1] == tailAge) tailWidth--;
            else if (gameField[tailHeight - 1, tailWidth + 1] == tailAge) tailWidth++;
            tailAge += incTail;
            Thread.Sleep(delayDuration);
        }
    }

    public void RefreshInterface() // Прорисовка интерфейса игры
    {
        int delayDuration = 1000;
        System.Console.Clear();
        System.Console.BackgroundColor = System.ConsoleColor.Black;
        for (int j = 0; j < gameWidth; j++)
        {
            System.Console.SetCursorPosition(j, 0);
            System.Console.Write("═");
            System.Console.SetCursorPosition(j, offsetTop);
            System.Console.Write("═");
            System.Console.SetCursorPosition(j, gameHeight - 1);
            System.Console.Write("═");
        }
        for (int i = 1; i < gameHeight - 1; i++)
        {
            System.Console.SetCursorPosition(0, i);
            System.Console.Write("║");
            System.Console.SetCursorPosition(gameWidth - 1, i);
            System.Console.Write("║");
        }
        System.Console.SetCursorPosition(0, 0);
        System.Console.Write("╔");
        System.Console.SetCursorPosition(gameWidth - 1, 0);
        System.Console.Write("╗");
        System.Console.SetCursorPosition(0, offsetTop);
        System.Console.Write("╠");
        System.Console.SetCursorPosition(gameWidth - 1, offsetTop);
        System.Console.Write("╣");
        System.Console.SetCursorPosition(0, gameHeight - 1);
        System.Console.Write("╚");
        System.Console.SetCursorPosition(gameWidth - 1, gameHeight - 1);
        System.Console.Write("╝");
        Thread.Sleep(delayDuration);
    }
}
class Application
{
    static void Main() // Основное тело программы
    {

        GameThreads gameProcess = new GameThreads();
        gameProcess.snakeDirection = "up";
        gameProcess.gameWidth = System.Console.WindowHeight * 2;
        gameProcess.gameHeight = System.Console.WindowHeight;
        gameProcess.offsetTop = 4;
        gameProcess.offsetInfo = 2;
        Thread interfaceThread = new Thread(new ThreadStart(gameProcess.RefreshInterface));
        interfaceThread.IsBackground = true;
        interfaceThread.Start();
        Thread snakeThread = new Thread(new ThreadStart(gameProcess.SnakeActions));
        snakeThread.IsBackground = true;
        snakeThread.Start();
        bool isActive = true;
        while (isActive)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) isActive = false;
            if (key.Key == ConsoleKey.UpArrow && gameProcess.snakeDirection != "down") gameProcess.snakeDirection = "up";
            if (key.Key == ConsoleKey.DownArrow && gameProcess.snakeDirection != "up") gameProcess.snakeDirection = "down";
            if (key.Key == ConsoleKey.RightArrow && gameProcess.snakeDirection != "left") gameProcess.snakeDirection = "right";
            if (key.Key == ConsoleKey.LeftArrow && gameProcess.snakeDirection != "right") gameProcess.snakeDirection = "left";
        }
    }
}