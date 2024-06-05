using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BankTransfer.Models;

namespace BankTransfer.Solutions
{
    public class AccountDictionarySolution
    {
        static ConcurrentDictionary<int, Object> locker = new ConcurrentDictionary<int, object>();
        public static void TransferHandler(AccountTransaction transaction)
        {
            var lower = Math.Min(transaction.From.AccountNumber, transaction.To.AccountNumber);
            var upper = Math.Max(transaction.From.AccountNumber, transaction.To.AccountNumber);

            var firstLock = locker.GetOrAdd(lower, num => new object());
            var secondLock = locker.GetOrAdd(upper, num => new object());
            

            lock (firstLock)
            lock (secondLock)
            {
                if (transaction.From.Balance < transaction.Amount)
                {
                    return;
                }
                Account.Transfer(transaction);
            }
        }
    }
}
