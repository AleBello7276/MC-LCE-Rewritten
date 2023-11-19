using System;
using System.Threading;

namespace net.minecraft.util
{
    public class AtomicLong
    {
        private long _value;
        private static readonly object lockObject = new object(); // Used for synchronization

        public AtomicLong(long value)
        {
            _value = value;
        }

        public AtomicLong() 
        {
        }

        public long get() {
            return _value;
        }


        public void set(long newValue) {
            _value = newValue;
        }

        public bool compareAndSet(long expect, long update)
        {
            // Use lock to ensure atomicity
            lock (lockObject)
            {
                if (_value == expect)
                {
                    _value = update;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public long Value
        {
            get { return Interlocked.Read(ref _value); }
            set { Interlocked.Exchange(ref _value, value); }
        }

        public long IncrementAndGet()
        {
            return Interlocked.Increment(ref _value);
        }

        public long DecrementAndGet()
        {
            return Interlocked.Decrement(ref _value);
        }

        public long AddAndGet(long delta)
        {
            return Interlocked.Add(ref _value, delta);
        }

        public bool CompareAndSet(long expected, long update)
        {
            return Interlocked.CompareExchange(ref _value, update, expected) == expected;
        }
    }
    
}


