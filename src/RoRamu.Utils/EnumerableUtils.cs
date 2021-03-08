namespace RoRamu.Utils
{
    using System;
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

        /// <summary>
        /// Tries to remove a given key-value pair from a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key in the key-value pair.</param>
        /// <param name="value">The value in the key-value pair.</param>
        /// <typeparam name="TKey">The type of the key in the key-value pair.</typeparam>
        /// <typeparam name="TValue">The type of the value in the key-value pair.</typeparam>
        /// <returns>True if the key-value pair was removed, otherwise false.</returns>
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
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
        /// Creates an equality comparer for a type from a lambda expression.
        /// </summary>
        /// <typeparam name="T">The type of the objects to be compared.</typeparam>
        /// <param name="equalsFunction">The function which checks whether two objects of the given type are equal.</param>
        /// <param name="getHashCodeFunction">The function which generates a hash code for the given type.</param>
        /// <returns>An equality comparer which uses the given functions to determine equality.</returns>
        public static IEqualityComparer<T> CreateEqualityComparer<T>(Func<T, T, bool> equalsFunction, Func<T, int> getHashCodeFunction = null)
        {
            return new GenericEqualityComparer<T>(equalsFunction, getHashCodeFunction);
        }

        /// <summary>
        /// Calculates the cross product of the given arrays.
        /// </summary>
        /// <param name="inputData">The arrays from which the cross product should be calculated.</param>
        /// <returns>The cross product. Each result can be accessed by iterating over the first index.</returns>
        public static object[][] CrossProduct(params object[][] inputData)
        {
            if (inputData == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }
            if (inputData.Length == 0)
            {
                throw new ArgumentException("Theory data must have at least 1 parameter.", nameof(inputData));
            }
            for (int i = 0; i < inputData.Length; i++)
            {
                if (inputData[i].Length <= 0)
                {
                    throw new ArgumentException($"The list of values for parameter at position {i} is empty.");
                }
            }

            // Iterate over each list to obtain each combination of parameter values
            List<object[]> result = new List<object[]>();
            int[] indexes = new int[inputData.Length];
            while (true)
            {
                // Construct the current row given the selected indexes
                object[] currentRow = new object[indexes.Length];
                for (int paramIndex = 0; paramIndex < inputData.Length; paramIndex++)
                {
                    int paramValueIndex = indexes[paramIndex];
                    currentRow[paramIndex] = inputData[paramIndex][paramValueIndex];
                }

                // Add this as a row in the data
                result.Add(currentRow);

                // Update the state of the indexes
                for (int i = 0; i < indexes.Length; i++)
                {
                    int val = indexes[i] + 1;
                    if (val < inputData[i].Length)
                    {
                        indexes[i] = val;
                        break;
                    }
                    else
                    {
                        indexes[i] = 0;
                    }
                }

                // Check if we're done
                foreach (int val in indexes)
                {
                    // If there is a non-zero value, we need to keep going
                    if (val != 0)
                    {
                        continue;
                    }
                }
                break;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Creates a comparer for a type from a lambda expression.
        /// </summary>
        /// <param name="compareFunction">The function which compares two objects of the given type.</param>
        /// <typeparam name="T">The type of the objects to be compared.</typeparam>
        /// <returns>A comparer which uses the given function to compare objects of the given type.</returns>
        public static IComparer<T> CreateComparer<T>(Func<T, T, int> compareFunction)
        {
            return new GenericComparer<T>(compareFunction);
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

        private class GenericComparer<T> : IComparer<T>
        {
            private readonly Func<T, T, int> _compare;

            public GenericComparer(Func<T, T, int> compareFunction)
            {
                this._compare = compareFunction ?? throw new ArgumentNullException(nameof(compareFunction));
            }

            public int Compare(T x, T y)
            {
                return this._compare(x, y);
            }
        }
    }
}
