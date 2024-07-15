using System;

namespace Game.Data
{
    public interface IMinMaxValue<T> where T : struct, IComparable, IFormattable, IConvertible, IEquatable<T>
    {
        public T MinValue { get; }
        public T MaxValue { get; }

        public T ClampValue(T value);
    }
}