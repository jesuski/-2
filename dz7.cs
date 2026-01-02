using System;

public class Level3_Password
{
    public static void Main(string[] args)
    {
        const string correctPassword = "12345";
        string inputPassword;

        do
        {
            Console.Write("Enter the password: ");
            inputPassword = Console.ReadLine();

            if (inputPassword != correctPassword)
            {
                Console.WriteLine("Incorrect password. Try again.");
            }

        } while (inputPassword != correctPassword);

        Console.WriteLine("\nAccess Granted! Welcome.");
    }
}