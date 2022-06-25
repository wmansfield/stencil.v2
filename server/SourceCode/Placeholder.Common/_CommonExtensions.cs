using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Placeholder.Common
{
    public static class _CommonExtensions
    {
        public static string TrimSafe(this string text)
        {
            if(text == null)
            {
                return string.Empty;
            }
            return text.Trim();
        }

        #region Date Extensions

        private static readonly DateTime EPOCH_START = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static int ToUnixSecondsUTC(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            TimeSpan ts = new TimeSpan(dateTime.ToUniversalTime().Ticks - EPOCH_START.Ticks);
            double seconds = Math.Round(ts.TotalMilliseconds, 0) / 1000.0;
            return (int)seconds;
        }
        public static DateTime? ToDateTimeFromUnixSecondsUTC(this long stamp)
        {
            if(stamp > 0)
            { 
                return EPOCH_START.AddSeconds(stamp);
            }
            return null;
        }

        public static int CalculateAgeOnDate(this DateTime birthDate, DateTime onDate)
        {
            int yearsPassed = onDate.Year - birthDate.Year;
            // Are we before the birth date this year? If so subtract one year from the mix
            if (onDate.Month < birthDate.Month || (onDate.Month == birthDate.Month && onDate.Day < birthDate.Day))
            {
                yearsPassed--;
            }
            return yearsPassed;
        }

        #endregion

        #region Encryption Extensions
        public static string HashAsString(this MD5 md5, string content)
        {
            StringBuilder sBuilder = new StringBuilder();

            if (md5 != null)
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
                for (int i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
            }
            return sBuilder.ToString();
        }
        public static string HashAsString(this SHA256 sha256, string content)
        {
            StringBuilder sBuilder = new StringBuilder();
            if (sha256 != null)
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
                for (int i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
            }
            return sBuilder.ToString();
        }

        #endregion

        #region Exception Extensions

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

        #region Dictionary Extensions

        public static string GetValueOrKey(this IDictionary<string, string> dictionary, string key)
        {
            string found = string.Empty;
            if (dictionary != null && !string.IsNullOrEmpty(key) && dictionary.TryGetValue(key, out found) && !string.IsNullOrEmpty(found))
            {
                return found;
            }
            return key;
        }

        #endregion

        #region Cosmos Dynamic Helpers

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var expr2Body = new RebindParameterVisitor(right.Parameters[0], left.Parameters[0]).Visit(right.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, expr2Body), left.Parameters);
        }

        private class RebindParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public RebindParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node == _oldParameter)
                {
                    return _newParameter;
                }

                return base.VisitParameter(node);
            }
        }

        #endregion
    }
}
