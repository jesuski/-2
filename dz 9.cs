using System;

public class Level3_GuessingGame
{
    public static void Main(string[] args)
    {
        Random random = new Random();
        int targetNumber = random.Next(1, 101); 

        int guess;
        int attempts = 0;
        bool guessedCorrectly = false;

        Console.WriteLine("I have chosen a number between 1 and 100. Start guessing!");

        while (!guessedCorrectly)
        {
            Console.Write("Enter your guess: ");
            attempts++;

            if (!int.TryParse(Console.ReadLine(), out guess))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                attempts--; 
                continue;
            }

            if (guess < targetNumber)
            {
                Console.WriteLine("The secret number is greater.");
            }
            else if (guess > targetNumber)
            {
                Console.WriteLine("The secret number is smaller.");
            }
            else
            {
                Console.WriteLine($"\nCongratulations! You guessed it in {attempts} attempts.");
                guessedCorrectly = true;
            }
        }
    }
}