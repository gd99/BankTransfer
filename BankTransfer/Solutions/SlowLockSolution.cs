using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankTransfer.Models;

namespace BankTransfer.Solutions
{
    public class SlowLockSolution
    {
        static object locker = new object();
        public static void TransferHandler(AccountTransaction transaction)
        {
            lock (locker)
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
