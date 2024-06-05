using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankTransfer.Models;

namespace BankTransfer.Solutions
{
    public class CircularBuffer
    {
        long circleBufferSize = 1000000;
        AccountTransaction[] transactions;
        int writeIndex = -1;
        int readIndex = 0;
        CancellationToken cancellationToken;
        CancellationTokenSource tokenSource;

        public CircularBuffer()
        {
            transactions = new AccountTransaction[circleBufferSize];
            
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;

            Task backgroundTask = Task.Run(() => RunTransactions(cancellationToken), cancellationToken);
        }

        public void Stop()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }

        public void TransferHandler(AccountTransaction transaction)
        {
            var currentWriteIndex = Interlocked.Increment(ref writeIndex);
            transactions[currentWriteIndex%circleBufferSize] = transaction;        
        }


        async Task RunTransactions(CancellationToken token)
        {
            //is anything at read value 
            //if so, run it, clear it and move on, 
            
            while(!token.IsCancellationRequested)
            {
                var transaction = transactions[readIndex];
                if (transaction != null)
                {        
                    if (transaction.From.Balance > transaction.Amount)
                    {
                        Account.Transfer(transaction);
                    }
                    
                    transactions[readIndex] = null;
                    readIndex++;
                }
                else
                {
                    await Task.Delay(50);
                }
            }
        }

      
    }
}
