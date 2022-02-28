﻿using System;
using System.Collections.Generic;

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
    }
}