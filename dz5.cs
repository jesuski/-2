using System;
public class sum
{
    public static void Main(string[] args)
    {
        Console.WriteLine(" Sum of Numbers (1 to N) ");

        Console.Write("Enter a positive integer N: ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a positive integer.");
            return;
        }
        long sum = 0;

        for (int i = 1; i <= n; i++)
        {
            sum += i;
        }
        Console.WriteLine($"The sum of numbers from 1 to {n} is: {sum}");
    }
}
