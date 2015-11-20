using System.Collections.Generic;
using System.Threading;

namespace Homework3{
    internal class BoundBufferWithMonitors<T>{
        //Contant variable initialized to 4000000;
        private const int MaxSize = 4000000;
        //Create private static List field.
        private static List<T> resultList = new List<T>();       
        //Create private static Queue field.
        private static Queue<T> numbersToCheck = new Queue<T>();       

        //Method to add numbers to a queue.
        public void Enqueue(T item){
            try{
                Monitor.Enter(numbersToCheck);
                while (numbersToCheck.Count >= MaxSize){
                    Monitor.Wait(numbersToCheck);
                }
                numbersToCheck.Enqueue(item);
                Monitor.Pulse(numbersToCheck);
            }finally{
                Monitor.Exit(numbersToCheck);
            }
        }

        //Method to remove numbers from a queue.
        public T Dequeue(){
            try { 
                Monitor.Enter(numbersToCheck);
                while(numbersToCheck.Count == 0){
                    Monitor.Wait(numbersToCheck);
                }
                var item = numbersToCheck.Dequeue();
                Monitor.Pulse(numbersToCheck);
                return item;
            }finally{
                Monitor.Exit(numbersToCheck);
            }         
        }

        //Get method to get queue.
        public Queue<T> getQueueForNumbersToCheck(){
            return numbersToCheck;
        }

        //Get method to get list.
        public List<T> getResultList(){
            return resultList;
        }

        //Set method to get list.
        public void setResultList(List<T> primeResults){
            lock (resultList){
                resultList = primeResults;
            }
        }

        //Set method to set Queue.
        public void setQueueForNumbersToCheck(Queue<T> numbersToQueue){
            lock (numbersToCheck){
                numbersToCheck = numbersToQueue;
            }
        }       
    }
}