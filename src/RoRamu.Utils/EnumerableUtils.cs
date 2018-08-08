namespace RoRamu.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// A set of utility methods to simplify operations related to enumerables.
    /// </summary>
    public static class EnumerableUtils
    {
        /// <summary>
        /// Creates an IEnumerable which contains the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="obj">The object</param>
        /// <returns>The IEnumerable.</returns>
        public static IEnumerable<T> SingleObjectAsEnumerable<T>(this T obj)
        {
            yield return obj;
        }

        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            
            if (!(dictionary is ICollection<KeyValuePair<TKey, TValue>> collection))
            {
                throw new ArgumentException($"Unable to cast {nameof(dictionary)} into an ICollection<KeyValuePair<{typeof(TKey).Name}, {typeof(TValue).Name}>>", nameof(dictionary));
            }

            var entry = new KeyValuePair<TKey, TValue>(key, value);
            return collection.Remove(entry);
        }

        /// <summary>
        /// Creates a comparer for a type from a lambda expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="equalsFunction"></param>
        /// <returns></returns>
        public static IEqualityComparer<T> CreateEqualityComparer<T>(Func<T, T, bool> equalsFunction, Func<T, int> getHashCodeFunction = null)
        {
            return new GenericEqualityComparer<T>(equalsFunction, getHashCodeFunction);
        }

        private class GenericEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _equals;
            private readonly Func<T, int> _getHashCode;

            public GenericEqualityComparer(Func<T, T, bool> equalsFunction, Func<T, int> getHashCodeFunction = null)
            {
                this._equals = equalsFunction ?? throw new ArgumentNullException(nameof(equalsFunction));
                this._getHashCode = getHashCodeFunction ?? (obj => obj.GetHashCode());
            }

            public bool Equals(T x, T y)
            {
                return this._equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return this._getHashCode(obj);
            }
        }
    }
}
