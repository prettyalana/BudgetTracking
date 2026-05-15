using System;
using System.Collections.Generic;

class Program
{
    static List<string> transactions = new List<string>();

    static void Main()
    {
        bool running = true;

        while (running)
        {
            ShowMenu();

            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewTransactions();
                    break;

                case "2":
                    AddTransaction();
                    break;

                case "3":

                    Console.WriteLine("Exiting program...");
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    // 1. User-Friendly Menu Interface
    static void ShowMenu()
    {
        Console.WriteLine("\n==== TRANSACTION MENU ====");
        Console.WriteLine("1. View Transactions");
        Console.WriteLine("2. Add Transaction");
        Console.WriteLine("3. Exit");
        Console.WriteLine("==========================");
    }

    // 2. View Transactions
    static void ViewTransactions()
    {
        Console.WriteLine("\n--- Transaction List ---");

        if (transactions.Count == 0)
        {
            Console.WriteLine("No transactions found.");

            return;
        }

        int index = 1;

        foreach (var t in transactions)
        {
            Console.WriteLine($"{index}. {t}");
            index++;
        }
    }

    // 3. Add Transactions
    static void AddTransaction()
    {
        Console.Write("Enter transaction description: ");
        string description = Console.ReadLine();

        Console.Write("Enter amount: ");
        string amountInput = Console.ReadLine();

        // Validation using conditional
        if (double.TryParse(amountInput, out double amount))
        {
            string record = $"{description} - {amount}";
            transactions.Add(record);

            Console.WriteLine("Transaction added successfully!");
        }

        else
        {
            Console.WriteLine("Invalid amount. Transaction not saved.");
        }
    }
}