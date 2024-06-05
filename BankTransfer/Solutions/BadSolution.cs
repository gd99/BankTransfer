using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BankTransfer.Models;

namespace BankTransfer.Solutions
{
    public class BadSolution
    {
        public static void TransferHandler(AccountTransaction transaction)
        {
            if (transaction.From.Balance < transaction.Amount)
            {
                return;
            }
            Account.Transfer(transaction);
        }

    }
}
