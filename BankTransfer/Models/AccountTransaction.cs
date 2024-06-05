using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransfer.Models
{
    public class AccountTransaction
    {
        public Account From;
        public Account To;
        public double Amount;

        public AccountTransaction(Account from, Account to, double amount)
        {
            From = from;
            To = to;
            Amount = amount;
        }
    }
}
