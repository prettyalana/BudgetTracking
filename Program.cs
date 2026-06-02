using System;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Net;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using Spectre.Console;

namespace BudgetManagementSystem
{
    class CannotBeEmptyException : Exception
    {
        public CannotBeEmptyException(string message) : base(message)
        {

        }
    }

    public class Transaction
    {
        // Properties
        public required string Description { get; set; }
        public decimal Amount { get; set; }

        public required Category CategoryName { get; set; }

        public DateTime Date { get; init; }

    }

    public class Category
    {
        public required string Name { get; init; }

        public decimal BudgetLimit { get; set; }

        public decimal RemainingBudget { get; set; }

    }

    class Program()
    {

        static List<Transaction> transactions = new List<Transaction>();
        static List<Category> categories = new()
        {
            new Category { Name = "Food" },
            new Category { Name = "Transportation" },
            new Category { Name = "Shopping" },
            new Category { Name = "Travel" },
            new Category { Name = "Miscellaneous" }
        };

        static decimal budgetLimit = new();


        static void Main()
        {
            bool exit = false;

            Greeting();

            while (!exit)
            {
                DisplayMenu();

                var userChoice = Console.ReadLine();

                if (int.TryParse(userChoice, out int intUserChoice))
                {
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid input. Please enter a number.[/]");
                }


                switch (intUserChoice)
                {
                    case 1:
                        ViewTransactions();
                        PromptUser();
                        break;
                    case 2:
                        AddTransactions();
                        PromptUser();
                        break;
                    case 3:
                        RemoveTransactions();
                        PromptUser();
                        break;
                    case 4:
                        Budget();
                        PromptUser();
                        break;
                    case 5:
                        ViewBudget();
                        PromptUser();
                        break;
                    case 6:
                        ExportTransactions();
                        PromptUser();
                        break;
                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        continue;
                }
            }

            Console.ReadKey();

            static void Greeting()
            {
                AnsiConsole.MarkupLine("\n[bold]Welcome to the 은행: Budget Tracker[/]");
                AnsiConsole.MarkupLine(@"[bold green]
............. ....      ............... 
... ............... ....................
.....#%######............-#%:....=##.=##
...%##:....-%#+......-##########.=##.=##
...##=......:##........##:..-##..=######
...*##-....-##=.......*#*....%#=.=##.=##
.....+######*..........##*..###..=##.=##
........................-###*:...=##.=##
##################=  .... ...-%######-..
..--................    .. .###.....*##.
..##:........ ......    ...:##.......##-
..##:.............. .   ....**#:...:##*.
..##############=..     ......=#####+...
...................     ............... [/]");
            }

            static void DisplayMenu()
            {
                Console.WriteLine("\n========================================");
                AnsiConsole.MarkupLine("[bold yellow]1. See all transactions[/]");
                AnsiConsole.MarkupLine("[bold green]2. Add a new transaction[/]");
                AnsiConsole.MarkupLine("[bold red]3. Remove a transaction[/]");
                AnsiConsole.MarkupLine("[bold deepskyblue1]4. Set a budget[/]");
                AnsiConsole.MarkupLine("[bold cyan]5. View budget[/]");
                AnsiConsole.MarkupLine("[bold deeppink1]6. Export transaction data[/]");
                AnsiConsole.MarkupLine("[bold magenta]7. Exit[/]");
                Console.WriteLine("========================================\n");
            }

            static void PrintSelectedOption(string selectedOption)
            {
                Console.WriteLine($"Selected option: {selectedOption}");
            }

            static void PromptUser()
            {
                Console.WriteLine("\nWhat do you want to do?");
            }

        }

        static void ViewTransactions()
        {
            Transaction allTransactions = transactions[0];
            if (transactions.Count == 0)
            {
                Console.WriteLine("\n No transactions have been added yet");
            }
            for (int i = 0; i < transactions.Count; i++)
            {
                {
                    Console.WriteLine($"{i + 1}. \t Description: {transactions[i].Description} \n \t Amount: ${transactions[i].Amount} \n \t Category: {transactions[i].CategoryName.Name} \n \t Date: {transactions[i].Date} \n");

                }
            }
        }

        static Category DisplayCategories()
        {

            Console.WriteLine("Please choose a category: ");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i].Name}");

            }
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out int selectedCategory))
            {
                return categories[selectedCategory - 1];
            }

            return null;
        }

        static void AddTransactions()
        {
            Console.WriteLine("Enter the transaction description: ");
            string description = "";
            decimal decimalAmount = 0;
            Category selectedCategory = null;
            DateTime currentTime = DateTime.Now;

            description = Console.ReadLine();

            try
            {
                if (string.IsNullOrWhiteSpace(description))
                {
                    throw new CannotBeEmptyException("The description cannot be empty.");

                }

            }
            catch (CannotBeEmptyException)
            {
                AnsiConsole.MarkupLine("[red]Description cannot be empty[/]");
            }

            while (true)
            {
                Console.WriteLine("Enter the transaction amount: ");
                string amount = Console.ReadLine(); // decimals need an m appended at the end if the value is money

                if (decimal.TryParse(amount, out decimalAmount))
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Please input a decimal value.[/]");
                    continue;
                }
            }

            selectedCategory = DisplayCategories();


            // Time 
            AnsiConsole.MarkupLine($"\n[green]Transaction has been successfully saved at: {currentTime}[/]\n");



            // Add the transaction
            Transaction newTransaction = new Transaction()
            {
                Description = description,
                Amount = decimalAmount,
                CategoryName = selectedCategory,
                Date = currentTime
            };

            transactions.Add(newTransaction);

            TransactionReport(selectedCategory, newTransaction, true);
        }

        static void TransactionReport(Category selectedCategoryName, Transaction transactionInfo, bool isAdding)
        {

            decimal budget = selectedCategoryName.BudgetLimit;
            decimal amountSpent = transactionInfo.Amount;
            decimal remaining = selectedCategoryName.RemainingBudget;

            if (isAdding == true)
            {
                selectedCategoryName.RemainingBudget = budget - amountSpent;
            }
            else
            {
                selectedCategoryName.RemainingBudget = selectedCategoryName.RemainingBudget + amountSpent;
            }

            AnsiConsole.MarkupLine($"[green]Budget: {budget}[/]");
            AnsiConsole.MarkupLine($"[red]Amount spent: {amountSpent}[/]");
            if (selectedCategoryName.RemainingBudget < 0)
            {
                AnsiConsole.MarkupLine($"[red]Remaining: {selectedCategoryName.RemainingBudget}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]Remaining: {selectedCategoryName.RemainingBudget}[/]");
            }

        }

        static void Budget()
        {
            // BudgetLimit
            Category selectedCategory = DisplayCategories();
            Console.WriteLine($"What is your budget for {selectedCategory.Name}?: ");
            string userBudget = Console.ReadLine();

            if (decimal.TryParse(userBudget, out decimal budgetLimit))
            {
                selectedCategory.BudgetLimit = budgetLimit;
                AnsiConsole.MarkupLine($"[green] Your budget for the {selectedCategory.Name} category is: ${budgetLimit}[/]");
            }

        }

        static void ViewBudget()
        {
            Console.WriteLine("========================================");
            foreach (Category categoryName in categories)
            {
                AnsiConsole.MarkupLine($"[springgreen1]{categoryName.Name}: {categoryName.BudgetLimit}[/]");
            }
            Console.WriteLine("========================================");

        }

        static void RemoveTransactions()
        {
            Transaction transactionToRemove = transactions[0];
            Console.WriteLine("Select the index of the transaction you want to remove: ");
            string removeTransaction = Console.ReadLine();
            if (int.TryParse(removeTransaction, out int removeTransactionInt))
            {
                transactionToRemove = transactions[removeTransactionInt - 1];
                transactions.RemoveAt(removeTransactionInt - 1);
                AnsiConsole.MarkupLine($"[red]\ntransaction removed: {removeTransaction}\n[/]");
            }
            else
            {
                Console.WriteLine("\nThe given index is not valid.\n");
            }


            TransactionReport(transactionToRemove.CategoryName, transactionToRemove, false);
        }
        static void ExportTransactions()
        {

            string path = "budgettracker.csv";

            StringBuilder output = new StringBuilder();

            string header = $"Description | Amount | Category | Date ";
            output.AppendLine(header);
            for (int i = 0; i < transactions.Count; i++)
            {
                
                string csvData = $"{transactions[i].Description}, ${transactions[i].Amount}, {transactions[i].CategoryName.Name}, {transactions[i].Date}";

                output.AppendLine(csvData);
            }

            header = "\nBudget";
            output.AppendLine(header);
            foreach (Category transactionCategory in categories)
            {
                string budget = $"{transactionCategory.Name}: {transactionCategory.BudgetLimit} \n";

                output.AppendLine(budget);
            }

            // TransactionReport(transactionToRemove.CategoryName, transactionToRemove, false);
            AnsiConsole.MarkupLine("[green]CSV file was successfully created.[/]");
            File.AppendAllText(path, output.ToString());



        }


    }
}