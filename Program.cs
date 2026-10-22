
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

enum TransactionType
{
    Income,
    Expense
}

class Transaction
{
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }

    public Transaction(string description, decimal amount, TransactionType type)
    {
        Description = description;
        Amount = amount;
        Type = type;
        Date = DateTime.Now;
    }
}

class Program
{
    static List<Transaction> transactions = new List<Transaction>();
    static string filePath = "transactions.json";

    static void Main()
    {
        LoadTransactions();

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
                    AddTransaction(TransactionType.Income);
                    break;

                case "2":
                    AddTransaction(TransactionType.Expense);
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
                    SaveTransactions();
                    Console.WriteLine("Data saved. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice!");
                    break;
            }
        }
    }

    static void AddTransaction(TransactionType type)
    {
        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Description cannot be empty!");
            return;
        }

        Console.Write("Amount: ");

        if (!decimal.TryParse(Console.ReadLine(), out decimal amount)
            || amount <= 0)
        {
            Console.WriteLine("Enter a valid positive amount!");
            return;
        }

        Transaction transaction =
            new Transaction(description, amount, type);

        transactions.Add(transaction);
        SaveTransactions();

        Console.WriteLine("Transaction added successfully!");
    }

    static void ShowTransactions()
    {
        if (transactions.Count == 0)
        {
            Console.WriteLine("No transactions found.");
            return;
        }

        for (int i = 0; i < transactions.Count; i++)
        {
            Transaction t = transactions[i];

            Console.WriteLine(
                $"{i + 1}. [{t.Type}] {t.Description} | " +
                $"{t.Amount:F2} AZN | {t.Date:dd.MM.yyyy HH:mm}"
            );
        }
    }

    static void ShowBalance()
    {
        decimal income = transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount);

        decimal expenses = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount);

        Console.WriteLine($"Total income: {income:F2} AZN");
        Console.WriteLine($"Total expenses: {expenses:F2} AZN");
        Console.WriteLine($"Balance: {income - expenses:F2} AZN");
    }

    static void ShowStatistics()
    {
        int incomeCount = transactions
            .Count(t => t.Type == TransactionType.Income);

        int expenseCount = transactions
            .Count(t => t.Type == TransactionType.Expense);

        Console.WriteLine($"Income transactions: {incomeCount}");
        Console.WriteLine($"Expense transactions: {expenseCount}");
        Console.WriteLine($"Total transactions: {transactions.Count}");

        if (transactions.Count > 0)
        {
            Transaction largest = transactions
                .OrderByDescending(t => t.Amount)
                .First();

            Console.WriteLine(
                $"Largest transaction: {largest.Description} " +
                $"({largest.Amount:F2} AZN, {largest.Type})"
            );
        }
    }

    static void DeleteTransaction()
    {
        ShowTransactions();

        if (transactions.Count == 0)
            return;

        Console.Write("Enter transaction number to delete: ");

        if (!int.TryParse(Console.ReadLine(), out int number)
            || number < 1
            || number > transactions.Count)
        {
            Console.WriteLine("Invalid transaction number!");
            return;
        }

        transactions.RemoveAt(number - 1);
        SaveTransactions();

        Console.WriteLine("Transaction deleted successfully!");
    }

    static void SaveTransactions()
    {
        string json = JsonSerializer.Serialize(
            transactions,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(filePath, json);
    }

    static void LoadTransactions()
    {
        if (!File.Exists(filePath))
            return;

        try
        {
            string json = File.ReadAllText(filePath);

            transactions =
                JsonSerializer.Deserialize<List<Transaction>>(json)
                ?? new List<Transaction>();
        }
        catch (JsonException)
        {
            Console.WriteLine("Warning: Saved data could not be read.");
            transactions = new List<Transaction>();
        }
        catch (IOException)
        {
            Console.WriteLine("Warning: Could not access the data file.");
        }
    }
}
