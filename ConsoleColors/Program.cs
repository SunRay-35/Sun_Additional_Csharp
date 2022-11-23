using System;

class Program
{
    static void Main()
    {
	//
	// This program demonstrates all colors and backgrounds.
	//
	Type type = typeof(ConsoleColor);
	Console.ForegroundColor = ConsoleColor.White;
	foreach (var name in Enum.GetNames(type))
	{
	    Console.BackgroundColor = (ConsoleColor)Enum.Parse(type, name);
	    Console.WriteLine(name);
	}
	Console.BackgroundColor = ConsoleColor.Black;
	foreach (var name in Enum.GetNames(type))
	{
	    Console.ForegroundColor = (ConsoleColor)Enum.Parse(type, name);
	    Console.WriteLine(name);
	}
    }
}