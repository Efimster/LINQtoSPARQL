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
        /// Select expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="items"></param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Select<T>(this ISPARQLQueryable<T> source, params string[] items)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(items, typeof(string[])) }));
        }
        /// <summary>
        /// Select expression. Projection
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Select<T>(this ISPARQLQueryable<T> source)
        {
            return source.Select(typeof(T));
        }
        /// <summary>
        /// Select expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="type">element type</param>
        /// <returns>query</returns>
        private static ISPARQLQueryable<T> Select<T>(this ISPARQLQueryable<T> source, Type type)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(type) }));
        }
    }
}
