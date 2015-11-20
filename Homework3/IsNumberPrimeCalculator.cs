using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework3
{
    internal class IsNumberPrimeCalculator
    {
        private readonly BoundBufferWithMonitors<long> IsNumberPrimeCalculatorboundBuffer;

        //Constructor for newly created thread.
        public IsNumberPrimeCalculator(List<long> primeNumbers, Queue<long> numbersToCheck){
            IsNumberPrimeCalculatorboundBuffer = new BoundBufferWithMonitors<long>();
            IsNumberPrimeCalculatorboundBuffer.setResultList(primeNumbers);
            IsNumberPrimeCalculatorboundBuffer.setQueueForNumbersToCheck(numbersToCheck);
        }
       
        //Method to check if numbers are prime numbers.
        public void CheckIfNumbersArePrime(){
            long numberToCheck = 0;
            //lock.
            while (true){
                try{
                    Monitor.Enter(IsNumberPrimeCalculatorboundBuffer.getQueueForNumbersToCheck());
                    while (IsNumberPrimeCalculatorboundBuffer.getQueueForNumbersToCheck().Count == 0){
                        Monitor.Wait(IsNumberPrimeCalculatorboundBuffer.getQueueForNumbersToCheck());
                    }
                    numberToCheck = IsNumberPrimeCalculatorboundBuffer.getQueueForNumbersToCheck().Dequeue();
                    Monitor.Pulse(IsNumberPrimeCalculatorboundBuffer.getQueueForNumbersToCheck());
                }
                finally{
                    Monitor.Exit(IsNumberPrimeCalculatorboundBuffer.getQueueForNumbersToCheck());
                }

                if (IsNumberPrime(numberToCheck)){
                    //lock.
                    lock (IsNumberPrimeCalculatorboundBuffer.getResultList()){
                        IsNumberPrimeCalculatorboundBuffer.getResultList().Add(numberToCheck);
                    }
                }
            }
        }


        private static bool IsNumberPrime(long numberWeAreChecking)
        {
            const long firstNumberToCheck = 3;

            if (numberWeAreChecking % 2 == 0)
            {
                return false;
            }
            var lastNumberToCheck = Math.Sqrt(numberWeAreChecking);
            for (var currentDivisor = firstNumberToCheck; currentDivisor < lastNumberToCheck; currentDivisor += 2)
            {
                if (numberWeAreChecking % currentDivisor == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}