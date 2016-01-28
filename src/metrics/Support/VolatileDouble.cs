﻿using System.Threading;

namespace metrics.Support
{
    /// <summary>
    /// Provides support for volatile operations around a <see cref="double" /> value
    /// </summary>
    public struct VolatileDouble
    {
        private double _value;

        public static VolatileDouble operator +(VolatileDouble left, VolatileDouble right)
        {
            return Add(left, right);
        }

        private static VolatileDouble Add(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() + right.Get());
            return left.Get();
        }

        public static VolatileDouble operator -(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() - right.Get());
            return left.Get();
        }

        public static VolatileDouble operator *(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() * right.Get());
            return left.Get();
        }

        public static VolatileDouble operator /(VolatileDouble left, VolatileDouble right)
        {
            left.Set(left.Get() / right.Get());
            return left.Get();
        }

        private VolatileDouble(double value) : this()
        {
            Set(value);
        }

        public void Set(double value)
        {
#if COREFX
            _value = value;
#else
            Thread.VolatileWrite(ref _value, value);
#endif
        }

        public double Get()
        {
#if COREFX
            return _value;
#else
            return Thread.VolatileRead(ref _value);
#endif
        }

        public static implicit operator VolatileDouble(double value)
        {
            return new VolatileDouble(value);
        }

        public static implicit operator double(VolatileDouble value)
        {
            return value.Get();
        }

        public override string ToString()
        {
            return Get().ToString();
        }
    }
}