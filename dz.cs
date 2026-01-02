using System;

public class 
    public static void Main(string[] args)
    {
        Console.WriteLine(" Arithmetic Operations );

        Console.Write("Enter the first number: ");
        if (!double.TryParse(Console.ReadLine(), out double num1))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            return;
        }

       
        Console.Write("Enter the second number: ");
        if (!double.TryParse(Console.ReadLine(), out double num2))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            return;
        }

        
        double sum = num1 + num2;
        double difference = num1 - num2;
        double product = num1 * num2;

        Console.WriteLine($"\nSum:        {num1} + {num2} = {sum}");
        Console.WriteLine($"Difference: {num1} - {num2} = {difference}");
        Console.WriteLine($"Product:    {num1} * {num2} = {product}");

       
        if (num2 != 0)
        {
            double quotient = num1 / num2;
            Console.WriteLine($"Quotient:   {num1} / {num2} = {quotient}");
        }
        else
        {
            Console.WriteLine("Quotient:   Cannot divide by zero.");
        }
    }
}
