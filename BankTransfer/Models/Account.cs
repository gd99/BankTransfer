using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankTransfer.Models
{
    public class Account
    {
        public int AccountNumber { get; init; }
        public double Balance { get; private set; }

        public Account(int accountNumber, double initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public static void Transfer(AccountTransaction transaction)
        {
            transaction.From.Balance -= transaction.Amount;
            transaction.To.Balance += transaction.Amount;
            if (transaction.From.Balance < 0)
            {
                //Failed
                throw new Exception("BROKE");
            }
        }
    }
}
