using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;


namespace LINQtoSPARQLSpace
{
    public static partial class LINQtoSPARQLExtensions
    {
        /// <summary>
        /// Close group expression. 
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> End<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression}));
        }

        /// <summary>
        /// Group expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Group<T>(this ISPARQLQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression }));
        }

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
            

                return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                    new Expression[] { source.Expression, Expression.Constant(items, typeof(string[]))}));
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

        /// <summary>
        /// Order By Expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="orderBy">order by  string expression</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> OrderBy<T>(this ISPARQLQueryable<T> source, string orderBy)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(orderBy)}));
        }
        /// <summary>
        /// Group By Expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="groupBy">"group by" expression</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> GroupBy<T>(this ISPARQLQueryable<T> source, string groupBy)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(groupBy) }));
        }
        /// <summary>
        /// Having expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="having">"having" expression</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Having<T>(this ISPARQLQueryable<T> source, string having)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(having) }));
        }

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
            
            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
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

        /// <summary>
        /// Converts to IEnumerable. Bridge to LINQ to Object
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>enumeration</returns>
        public static IEnumerable<T> AsEnumerable<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (IEnumerable<T>)source.Provider.ExecuteEnumerable<T>(source.Expression);
        }
        /// <summary>
        /// Prefix expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="prefix">prefix</param>
        /// <param name="iri">iri</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Prefix<T>(this ISPARQLQueryable<T> source, string prefix, string iri)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(prefix), Expression.Constant(iri) }));
        }


    }


}
