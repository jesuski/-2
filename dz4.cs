using System;

public class divisibility
{
    public static void Main(string[] args)
    {
   
        Console.WriteLine(" Divisibility by 4 and 6 Check ");

        Console.Write("Enter an integer: ");
        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            return;
        }

        bool divisibleBy4 = (number % 4 == 0);
        bool divisibleBy6 = (number % 6 == 0);

        Console.WriteLine($"\nResults for {number}:");

        if (divisibleBy4 && divisibleBy6)
        {
            Console.WriteLine($"The number {number} is divisible by BOTH 4 and 6.");
        }
        else if (divisibleBy4)
        {
            Console.WriteLine($"The number {number} is divisible only by 4.");
        }
        else if (divisibleBy6)
        {
            Console.WriteLine($"The number {number} is divisible only by 6.");
        }
        else
        {
            Console.WriteLine($"The number {number} is not divisible by 4 or 6.");
        }
    }
}