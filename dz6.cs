using System;

public class MultiplicationTable
{
    public static void Main(string[] args)
    {
        Console.Write("Enter an integer N: ");
        if (!int.TryParse(Console.ReadLine(), out int n))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            return;
        }

        Console.WriteLine($"\nMultiplication Table for {n}:");

        for (int i = 1; i <= 10; i++)
        {
            int result = i * n;
            Console.WriteLine($"{i} x {n} = {result}");
        }
    }
}
