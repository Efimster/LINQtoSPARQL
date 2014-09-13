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
        public static ISPARQLMatchQueryable<T> End<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression}));
        }

        /// <summary>
        /// Group expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Group<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression }));
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
        /// <summary>
        /// Bind expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="binding">binding</param>
        /// <returns>query</returns>
        public static ISPARQLBindingQueryable<T> Bind<T>(this ISPARQLQueryable<T> source, string binding)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (ISPARQLBindingQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(binding) }));
        }
        /// <summary>
        /// "As" expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="value">as</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> As<T>(this ISPARQLBindingQueryable<T> source, string value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (ISPARQLMatchQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(value) }));
        }
        /// <summary>
        /// Distinct expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Distinct<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression }));
        }



    }


}
