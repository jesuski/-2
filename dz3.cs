using System;

public class age
{
    public static void Main(string[] args)
    {
        Console.WriteLine(" Age Categorization ");

        Console.Write("Enter your age: ");
        if (!int.TryParse(Console.ReadLine(), out int age) || age < 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid non-negative integer for age.");
            return;
        }
        string category;

        if (age < 12)
        {
            category = "Child";         }
        else if (age >= 12 && age <= 17)
        {
            category = "Teenager"; 
        }
        else 
        {
            category = "Adult"; 
        }
        Console.WriteLine($"You are classified as: {category}.");
    }
}
