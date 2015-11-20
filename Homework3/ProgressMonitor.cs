using System;
using System.Collections.Generic;
using System.Threading;

namespace Homework3
{
    internal class ProgressMonitor
    {
        private BoundBufferWithMonitors<long> progressMonitorBuffer;
        public long TotalCount = 0;

        public ProgressMonitor(List<long> results)
        {
            progressMonitorBuffer = new BoundBufferWithMonitors<long>();
            progressMonitorBuffer.setResultList(results);
        }

        public void Run()
        {
            while (true)
            {
                Thread.Sleep(100);

                lock (progressMonitorBuffer.getResultList())
                {
                    var currentcount = progressMonitorBuffer.getResultList().Count;
                    TotalCount += currentcount;

                    progressMonitorBuffer.getResultList().Clear(); // clear out the current primes to save some memory

                    if (currentcount > 0)
                        Console.WriteLine("{0} primes found so far", TotalCount);
                }
            }
        }
    }
}