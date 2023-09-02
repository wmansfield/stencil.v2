using Stencil.Common.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stencil.Forms
{
    public static class _CoreExtensions
    {
        public static T DisposeSafe<T>(this T disposable)
            where T : class, IDisposable
        {
            disposable?.Dispose();
            return null;
        }

        #region Error Methods

        public static TException FirstExceptionOfType<TException>(this Exception ex)
            where TException : Exception
        {
            TException result = ex as TException;
            if (result != null)
            {
                return result;
            }
            AggregateException aggregate = ex as AggregateException;
            if (aggregate != null)
            {
                foreach (var item in aggregate.InnerExceptions)
                {
                    result = FirstExceptionOfType<TException>(item);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
        public static Exception FirstNonAggregateException(this Exception ex)
        {
            AggregateException aggregate = ex as AggregateException;
            if (aggregate != null)
            {
                foreach (var item in aggregate.InnerExceptions)
                {
                    return FirstNonAggregateException(item);
                }
            }
            return ex;
        }

        #endregion


        #region KeyValuePair Methods

        public static void Replace(this List<KeyValuePair<string, string>> list, string key, string value)
        {
            if (list != null)
            {
                int found = list.FindIndex(x => x.Key == key);
                if (found >= 0)
                {
                    if (list[found].Value == value)
                    {
                        return; // Short Circuit, the same
                    }
                    list.RemoveAt(found);
                }
                list.Add(new KeyValuePair<string, string>(key, value));
            }

        }
        public static void Remove(this List<KeyValuePair<string, string>> list, string key)
        {
            if (list != null)
            {
                int found = list.FindIndex(x => x.Key == key);
                if (found >= 0)
                {
                    list.RemoveAt(found);
                }
            }
        }
        #endregion

        /// <summary>
        /// Assigns the value at the specified index, expands the array if needed
        /// </summary>
        public static T[] InjectAt<T>(this T[] array, int index, T entity)
        {
            if (array != null && index < array.Length)
            {
                array[index] = entity;
                return array;
            }
            else
            {
                if (array == null)
                {
                    array = new T[index + 1];
                    array[index] = entity;
                    return array;
                }
                else
                {
                    T[] result = new T[index + 1];
                    array.CopyTo(result, 0);
                    array[index] = entity;
                    return result;
                }
            }
        }

        [Obsolete("Are you sure you want to wait synchronously?", false)]
        public static TResult SyncResult<TResult>(this Task<TResult> task)
        {
            return task.GetAwaiter().GetResult();
        }

        public static LayoutOptions ToLayoutOptions(this LayoutAlignmentInfo alignmentInfo)
        {
            switch (alignmentInfo)
            {
                case LayoutAlignmentInfo.Start:
                    return LayoutOptions.Start;
                case LayoutAlignmentInfo.Center:
                    return LayoutOptions.Center;
                case LayoutAlignmentInfo.End:
                    return LayoutOptions.End;
                case LayoutAlignmentInfo.Fill:
                    return LayoutOptions.Fill;
                default:
                    return LayoutOptions.Start;
            }
        }
        public static LayoutAlignmentInfo ToAlignmentInfo(this LayoutOptions options)
        {
            switch (options.Alignment)
            {
                case LayoutAlignment.Start:
                    return LayoutAlignmentInfo.Start;
                case LayoutAlignment.Center:
                    return LayoutAlignmentInfo.Center;
                case LayoutAlignment.End:
                    return LayoutAlignmentInfo.End;
                case LayoutAlignment.Fill:
                    return LayoutAlignmentInfo.Fill;
                default:
                    return LayoutAlignmentInfo.Start;
            }
        }
    }
}
