using System;
using System.Collections.Generic;
using System.IO;
//using System.Threading;

namespace Homework3
{
    class NumberReader : IDisposable
    {
        //Create new BoundBufferWithMonitors object.
        private readonly BoundBufferWithMonitors<long> NumberReaderboundBuffer = new BoundBufferWithMonitors<long>();
        private readonly TextReader _reader;

        public NumberReader(FileInfo file)
        {
            _reader = new StreamReader(new BufferedStream(new FileStream(
                file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan), 65536));
        }

        public void ReadIntegers(Queue<long> numbersToCheck){
            String line;
            string[] stringOfIntgers = _reader.ReadToEnd().Split('\n');
            int length = stringOfIntgers.Length;
            if (length > 0){
                for (int count = 0; count < length - 1; count++){
                    line = stringOfIntgers[count].TrimEnd('\r');
                    var value = long.Parse(line);
                    NumberReaderboundBuffer.Enqueue(value);
                }
            }
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}