using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BankTransfer.Models;
using BankTransfer.Solutions;

namespace BankTransfer
{
    public class RunTests
    {
        public static void Run()
        {
            Console.WriteLine("Starting slow lock");
           
            RunLotsOfTransfersParallel(SlowLockSolution.TransferHandler);

            Console.WriteLine("Starting dictionary lock");
            RunLotsOfTransfersParallel(AccountDictionarySolution.TransferHandler);

            Console.WriteLine("Starting Circular buffer");

            var cv = new CircularBuffer();
            RunLotsOfTransfersParallel(cv.TransferHandler);
            cv.Stop();

            Console.WriteLine("Single threaded");


            //baseline 
            RunLotsOfTransfersSingleThread(BadSolution.TransferHandler);


            Console.WriteLine("Done");
        }


        public static void RunLotsOfTransfersSingleThread(Action<AccountTransaction> solution)
        {
            Stopwatch sw = new Stopwatch();
            int accountQuantity = 10000;
            int transactions = 10000000;
            double transferLimit = 300;
            var accounts = new List<Account>(accountQuantity);
            var randomBalance = new Random(1);

            for (int i = 0; i < accountQuantity; i++)
            {
                accounts.Add(new Account(i, randomBalance.Next(0, 500)));
            }
            sw.Start();
            Parallel.For(0, transactions, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, x =>
            {
                var randomAccount = new Random();
                var transferFromTo = TwoRandomDifferentAccounts(randomAccount, accounts);
                var accountFrom = transferFromTo.Item1;
                var accountTo = transferFromTo.Item2;
                var amount = randomBalance.NextDouble() * transferLimit;

                solution(new AccountTransaction(accountFrom, accountTo, amount));
            });

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        public static void RunLotsOfTransfersParallel(Action<AccountTransaction> solution)
        {
            Stopwatch sw = new Stopwatch();
            int accountQuantity = 10000;
            int transactions = 10000000;
            double transferLimit = 300;
            var accounts = new List<Account>(accountQuantity);
            var randomBalance = new Random(1);

            for (int i = 0; i < accountQuantity; i++)
            {
                accounts.Add(new Account(i, randomBalance.Next(0, 500)));
            }
            sw.Start();
            Parallel.For(0, transactions, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, x =>
            {
                var randomAccount = new Random();
                var transferFromTo = TwoRandomDifferentAccounts(randomAccount, accounts);
                var accountFrom = transferFromTo.Item1;
                var accountTo = transferFromTo.Item2;
                var amount = randomBalance.NextDouble() * transferLimit;

                solution(new AccountTransaction(accountFrom, accountTo, amount));
            });

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

       

        public static (Account, Account) TwoRandomDifferentAccounts(Random random, List<Account> accounts)
        {
            var accountNum1 = random.Next(accounts.Count);
            var accountNum2 = random.Next(accounts.Count);

            while(accountNum1 == accountNum2)
            {
                accountNum2 = random.Next(accounts.Count);
            }

            return (accounts[accountNum1], accounts[accountNum2]);
        }
    }

    
}
