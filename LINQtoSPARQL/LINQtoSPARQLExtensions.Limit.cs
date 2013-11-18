using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSPARQLSpace
{
    public partial class LINQtoSPARQLExtensions
    {
        /// <summary>
        /// Limit expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="number">limit number</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Limit<T>(this ISPARQLQueryable<T> source, int number)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(number) }));
        }
        /// <summary>
        /// Offset expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="number">offset number</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Offset<T>(this ISPARQLQueryable<T> source, int number)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(number) }));
        }
    }
}
