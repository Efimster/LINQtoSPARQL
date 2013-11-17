using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HelperExtensionsLibrary.Strings;

namespace LINQtoSPARQLSpace
{
    public static partial class LINQtoSPARQLExtensions
    {
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, string s, string p, string o)
        {
            return source.Match<T>(s, p, o);
        }

        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, string p, string o)
        {
            return (ISPARQLMatchedQueryable<T>)source.And_2<T>(p, o);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        private static ISPARQLQueryable<T> And_2<T>(this ISPARQLQueryable source, string p, string o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(p), Expression.Constant(o) }));
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, string o)
        {
            return (ISPARQLMatchedQueryable<T>)source.And_1<T>(o);
        }

        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        private static ISPARQLQueryable And_1<T>(this ISPARQLQueryable source, string o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(o) }));
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, Expression<Func<T, dynamic>> s, string p, string o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <param name="o"></param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, string s, Expression<Func<T, dynamic>> p, string o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source"></param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns></returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, string s, string p, Expression<Func<T, dynamic>> o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, string o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, Expression<Func<T, dynamic>> s, string p, Expression<Func<T, dynamic>> o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, string s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, Expression<Func<T, dynamic>> p, string o)
        {
            string pName = "?" + ((MemberExpression)p.Body).Member.Name.ToLower();
            return (ISPARQLMatchedQueryable<T>)source.And_2<T>(pName, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            string pName = "?" + ((MemberExpression)p.Body).Member.Name.ToLower();
            string oName = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            return (ISPARQLMatchedQueryable<T>)source.And_2<T>(pName, oName);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> And<T>(this ISPARQLMatchedQueryable<T> source, string p, Expression<Func<T, dynamic>> o)
        {
            string oName = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            return (ISPARQLMatchedQueryable<T>)source.And_2<T>(p, oName);
        }

    }
}
