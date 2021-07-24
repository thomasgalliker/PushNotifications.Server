using System;
using System.Collections.Generic;

namespace PushNotifications.Internals
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Iterates two enumerabes <paramref name="first"/> and <paramref name="second"/> in parallel 
        /// and calls <paramref name="action"/> with the counterpart element of each enumerable.
        /// </summary>
        public static void ForPair<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Action<TFirst, TSecond> action)
        {
            using (var enumeratorFirst = first.GetEnumerator())
            using (var enumeratorSecond = second.GetEnumerator())
            {
                while (enumeratorFirst.MoveNext() && enumeratorSecond.MoveNext())
                {
                    action(enumeratorFirst.Current, enumeratorSecond.Current);
                }
            }
        }
    }
}
