using System;
using System.Text;
using Spectre.Console;

namespace BudgetManagementSystem
{

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

    }

    public class Budget
    {

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
                        AnsiConsole.MarkupLine("[red]Invalid choice. Please select a number between 1 and 7.[/]");
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
                AnsiConsole.MarkupLine("[bold lightpink1]6. Export transaction data[/]");
                AnsiConsole.MarkupLine("[bold deeppink1]7. Exit[/]");
                Console.WriteLine("========================================\n");
            }

            static void PromptUser()
            {
                Console.WriteLine("\nWhat do you want to do?");
            }

        }

        static void ViewTransactions()
        {
            if (transactions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No transactions have been added yet.[/]");
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

            while (true)
            {
                Console.WriteLine("\nPlease choose a category: ");


                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i].Name}");

                }

                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int selectedCategory) && selectedCategory > 0 && selectedCategory <= categories.Count)
                {
                    return categories[selectedCategory - 1];
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]\nThe given index is not valid. Please choose a number between 1 and 5.\n[/]");
                    continue;
                }
            }

        }

        static void AddTransactions()
        {
            string description = "";
            decimal decimalAmount = 0;
            Category selectedCategory = null;
            DateTime currentTime = DateTime.Now;

            while (true)
            {
                Console.WriteLine("Enter the transaction description: ");
                description = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(description))
                {
                    AnsiConsole.MarkupLine("[red]Description cannot be empty[/]");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.WriteLine("\nEnter the transaction amount: ");
                string amount = Console.ReadLine();

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


            if (selectedCategory.BudgetLimit == 0)
            {

            }
            else
            {
                TransactionReport(selectedCategory, newTransaction);
            }
        }

        static void TransactionReport(Category selectedCategoryName, Transaction transactionInfo)
        {

            decimal budget = selectedCategoryName.BudgetLimit;
            decimal amountSpent = DynamicBudget(selectedCategoryName);

            decimal remaining = budget - amountSpent;

            AnsiConsole.MarkupLine($"[green]Budget: {budget}[/]");
            AnsiConsole.MarkupLine($"[red]Amount spent: {amountSpent}[/]");
            if (remaining < 0)
            {
                AnsiConsole.MarkupLine($"[red]Remaining: {remaining}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]Remaining: {remaining}[/]");
            }

        }

        static void Budget()
        {
            // BudgetLimit
            Category selectedCategory = DisplayCategories();
            Console.WriteLine($"What is your budget for {selectedCategory.Name}?: ");
            string userBudget = Console.ReadLine();

            decimal transactionTotal = DynamicBudget(selectedCategory);

            if (decimal.TryParse(userBudget, out decimal budgetLimit))
            {
                selectedCategory.BudgetLimit = budgetLimit;
                decimal budgetTotal = budgetLimit - transactionTotal;

                AnsiConsole.MarkupLine($"[green] Your budget for the {selectedCategory.Name} category is: ${userBudget}[/]");

                if (transactionTotal > 0 && budgetTotal > 0)
                {
                    AnsiConsole.MarkupLine($"[green] You have ${budgetTotal} left to spend.[/]");
                }
                else if (budgetTotal < 0)
                {
                    AnsiConsole.MarkupLine($"[red] You are ${Math.Abs(budgetTotal)} over your budget.[/]");
                }
                else if (budgetTotal == 0)
                {
                    AnsiConsole.MarkupLine($"[yellow] Your budget is now ${budgetTotal}. You have reached your budget limit.[/]");
                }
            }


        }

        // This method is needed for transactionreport, budget, view budget, and export transactions
        static decimal DynamicBudget(Category categoryBudget)
        {
            decimal transactionTotal = 0;
            foreach (Transaction item in transactions)
            {
                if (item.CategoryName.Name.ToString() == categoryBudget.Name)
                {
                    transactionTotal += item.Amount;
                }
            }

            return transactionTotal;
        }

        static void ViewBudget()
        {
            Console.WriteLine("========================================");
            foreach (Category categoryName in categories)
            {
                AnsiConsole.MarkupLine($"[springgreen1]{categoryName.Name}: Budget: ${categoryName.BudgetLimit} | Amount spent: ${DynamicBudget(categoryName)}\n[/]");
            }
            Console.WriteLine("========================================");

        }

        static void RemoveTransactions()
        {
            if (transactions.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]There are no transactions to remove.[/]");
                return;
            }

            Transaction transactionToRemove = null;

            while (true)
            {
                Console.WriteLine("Select the index of the transaction you want to remove: ");
                string removeTransaction = Console.ReadLine();
                if (int.TryParse(removeTransaction, out int removeTransactionInt) && removeTransactionInt > 0 && removeTransactionInt <= transactions.Count)
                {
                    transactionToRemove = transactions[removeTransactionInt - 1];
                    transactions.RemoveAt(removeTransactionInt - 1);
                    AnsiConsole.MarkupLine($"[red]\ntransaction removed: {removeTransaction}\n[/]");
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]\nThe given index is not valid.\n[/]");
                    continue;
                }
            }


            TransactionReport(transactionToRemove.CategoryName, transactionToRemove);
        }
        static void ExportTransactions()
        {

            try
            {
                string path = "budgettracker.csv";

                StringBuilder output = new StringBuilder();

                string header = "Description, Category, Amount, Date";
                output.AppendLine(header);

                for (int i = 0; i < transactions.Count; i++)
                {

                    string csvData = $"{transactions[i].Description}, {transactions[i].CategoryName.Name}, {transactions[i].Amount}, {transactions[i].Date}";

                    output.AppendLine(csvData);
                }

                File.AppendAllText(path, output.ToString());
                AnsiConsole.MarkupLine("[green]CSV file was successfully created.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]Export failed[/]");
                AnsiConsole.WriteException(ex);
            }



        }


    }
}