using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<string> descriptions = new List<string>();
    static List<decimal> amounts = new List<decimal>();
    static List<string> types = new List<string>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n===== EXPENSE TRACKER =====");
            Console.WriteLine("1. Add income");
            Console.WriteLine("2. Add expense");
            Console.WriteLine("3. Show all transactions");
            Console.WriteLine("4. Show balance");
            Console.WriteLine("5. Show statistics");
            Console.WriteLine("6. Delete transaction");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    AddTransaction("Income");
                    break;

                case "2":
                    AddTransaction("Expense");
                    break;

                case "3":
                    ShowTransactions();
                    break;

                case "4":
                    ShowBalance();
                    break;

                case "5":
                    ShowStatistics();
                    break;

                case "6":
                    DeleteTransaction();
                    break;

                case "0":
                    Console.WriteLine("Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static void AddTransaction(string type)
    {
        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Description cannot be empty!");
            return;
        }

        Console.Write("Amount: ");
        decimal amount;

        if (!decimal.TryParse(Console.ReadLine(), out amount)
            || amount <= 0)
        {
            Console.WriteLine("Enter a valid positive amount!");
            return;
        }

        descriptions.Add(description);
        amounts.Add(amount);
        types.Add(type);

        Console.WriteLine("Transaction added successfully!");
    }

    static void ShowTransactions()
    {
        if (descriptions.Count == 0)
        {
            Console.WriteLine("No transactions found.");
            return;
        }

        for (int i = 0; i < descriptions.Count; i++)
        {
            Console.WriteLine(
                (i + 1) + ". [" + types[i] + "] " +
                descriptions[i] + " | " +
                amounts[i].ToString("F2") + " AZN"
            );
        }
    }

    static void ShowBalance()
    {
        decimal income = 0;
        decimal expenses = 0;

        for (int i = 0; i < amounts.Count; i++)
        {
            if (types[i] == "Income")
                income += amounts[i];
            else
                expenses += amounts[i];
        }

        Console.WriteLine("Total income: " + income.ToString("F2") + " AZN");
        Console.WriteLine("Total expenses: " + expenses.ToString("F2") + " AZN");
        Console.WriteLine("Balance: " + (income - expenses).ToString("F2") + " AZN");
    }

    static void ShowStatistics()
    {
        int incomeCount = types.Count(t => t == "Income");
        int expenseCount = types.Count(t => t == "Expense");

        Console.WriteLine("Income transactions: " + incomeCount);
        Console.WriteLine("Expense transactions: " + expenseCount);
        Console.WriteLine("Total transactions: " + descriptions.Count);

        if (amounts.Count > 0)
        {
            int largestIndex = 0;

            for (int i = 1; i < amounts.Count; i++)
            {
                if (amounts[i] > amounts[largestIndex])
                    largestIndex = i;
            }

            Console.WriteLine(
                "Largest transaction: " +
                descriptions[largestIndex] + " (" +
                amounts[largestIndex].ToString("F2") + " AZN)"
            );
        }
    }

    static void DeleteTransaction()
    {
        ShowTransactions();

        if (descriptions.Count == 0)
            return;

        Console.Write("Enter transaction number to delete: ");
        int number;

        if (!int.TryParse(Console.ReadLine(), out number)
            || number < 1
            || number > descriptions.Count)
        {
            Console.WriteLine("Invalid transaction number!");
            return;
        }

        int index = number - 1;

        descriptions.RemoveAt(index);
        amounts.RemoveAt(index);
        types.RemoveAt(index);

        Console.WriteLine("Transaction deleted successfully!");
    }
}
