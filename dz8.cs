using System;

public class Password
{
    public static void Main(string[] args)
    {
        const string correctPassword = "040869";
        string inputPassword;

        do
        {
            Console.Write("Enter the password: ");
            inputPassword = Console.ReadLine();

            if (inputPassword != correctPassword)
            {
                Console.WriteLine("Incorrect password try again");
            }

        } while (inputPassword != correctPassword);

        Console.WriteLine("\nAccess granted welcome.");
    }
}
