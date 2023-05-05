using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {

            ReportGenerator reportGenerator = new ReportGenerator();
            TransactionList transactionList = new TransactionList();

            while (true)
            {
                Console.WriteLine("Expense Tracker");
                Console.WriteLine("----------------");

                Console.WriteLine("1. Add expense");
                Console.WriteLine("2. Add income");
                Console.WriteLine("3. View transactions");
                Console.WriteLine("4. Export file report");
                Console.WriteLine("5. Export console report");
                Console.WriteLine("6. Clear all transactions");
                Console.WriteLine("7. Exit");

                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Add expense
                        transactionList.AddTransaction(TransactionType.Expense);

                        break;

                    case "2":

                        transactionList.AddTransaction(TransactionType.Income);

                        break;

                    case "3":
                        // View transactions
                        transactionList.ViewTransactions();

                        break;

                    case "4":
                        // Export report                     
                        reportGenerator.GenerateFileReport(transactionList);

                        break;

                    case "5":
                        // Export report                     
                        reportGenerator.GenerateConsoleReport(transactionList);

                        break;


                    case "6":
                        // Clear all transactions
                        transactionList.ClearTransactions();

                        break;

                    case "7":
                        // Exit the application
                        Console.WriteLine("Thank you for using Expense Tracker!");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }
    }

    // Define the Transaction class
    class Transaction
    {
        public decimal Amount { get; }
        public string Category { get; }
        public TransactionType Type { get; }

        public Transaction(decimal amount, string category, TransactionType type)
        {
            Amount = amount;
            Category = category;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Category}: {Amount}$ ({Type})";
        }
    }

    class TransactionList
    {
        public List<Transaction> transactions { get; set; }

        public TransactionList()
        {
            transactions = new List<Transaction>();
        }

        public void AddTransaction(TransactionType type)
        {
            Console.Write("Enter amount: ");
            var expenseAmount = decimal.Parse(Console.ReadLine());
            Console.Write("Enter category: ");
            var expenseCategory = Console.ReadLine();

            transactions.Add(new Transaction(expenseAmount, expenseCategory, type));

            Console.WriteLine("Transaction added successfully.");
        }

        public void ClearTransactions()
        {
            transactions.Clear();

            Console.WriteLine("All transactions cleared.");
        }

        public void ViewTransactions()
        {
            Console.WriteLine("Transactions:");
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
            }
        }

    }

    // Define the TransactionType enum
    enum TransactionType
    {
        Expense,
        Income
    }

    interface IDataFormatter
    {
        public void FormatData(List<Transaction> transactionList);
    }


    class ConsoleFormatter : IDataFormatter
    {
        public void FormatData(List<Transaction> transactions)
        {
            Console.WriteLine("Expense Tracker Report");
            Console.WriteLine("-----------------------");
            Console.WriteLine("Transactions:");

            decimal totalExpenses = 0;
            decimal totalIncome = 0;

            foreach (Transaction transaction in transactions)
            {
                if (transaction.Type == TransactionType.Expense)
                {
                    Console.WriteLine($"Expense: {transaction.Category} - {transaction.Amount} $");
                    totalExpenses += transaction.Amount;
                }
                else
                {
                    Console.WriteLine($"Income: {transaction.Category} - {transaction.Amount} $");
                    totalIncome += transaction.Amount;
                }
            }

            Console.WriteLine("");
            Console.WriteLine($"Total Expenses: {totalExpenses} $");
            Console.WriteLine($"Total Income: {totalIncome} $");
            Console.WriteLine($"Net Balance: {(totalIncome - totalExpenses)} $");
        }
    }

    class FileFormatter : IDataFormatter
    {
        public void FormatData(List<Transaction> transactions)
        {
            Console.Write("Enter file name: ");
            string fileName = Console.ReadLine();

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(fileName))
            {
                decimal totalExpenses = 0;
                decimal totalIncome = 0;

                file.WriteLine("Expense Tracker Report");
                file.WriteLine("-----------------------");
                file.WriteLine("Transactions:");

                foreach (Transaction transaction in transactions)
                {
                    if (transaction.Type == TransactionType.Expense)
                    {
                        file.WriteLine($"Expense: {transaction.Category} - {transaction.Amount} $");
                        totalExpenses += transaction.Amount;
                    }
                    else
                    {
                        file.WriteLine($"Income: {transaction.Category} - {transaction.Amount} $");
                        totalIncome += transaction.Amount;
                    }
                }

                file.WriteLine("");
                file.WriteLine($"Total Expenses: {totalExpenses} $");
                file.WriteLine($"Total Income: {totalIncome} $");
                file.WriteLine($"Net Balance: {(totalIncome - totalExpenses)} $");
            }

        }
    }

    class ReportGenerator
    {
        public ReportGenerator() { }

        public void GenerateConsoleReport(TransactionList transactionList)
        {
            var consoleFormatter = new ConsoleFormatter();
            consoleFormatter.FormatData(transactionList.transactions);
        }

        public void GenerateFileReport(TransactionList transactionList)
        {
            var fileFormatter = new FileFormatter();
            fileFormatter.FormatData(transactionList.transactions);
        }
    }

}
