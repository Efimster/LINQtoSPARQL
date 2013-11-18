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
    public partial class LINQtoSPARQLExtensions
    {
        /// <summary>
        /// Minus expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Minus<T>(this ISPARQLQueryable<T> source, string s, string p, string o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(s, typeof(string)), Expression.Constant(p), Expression.Constant(o) }));
        }
        /// <summary>
        /// Minus expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Minus<T>(this ISPARQLUnionQueryable<T> source, string s, string p, string o)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Minus<T>(s, p, o);
        }

        /// <summary>
        /// Minus expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Minus<T>(this ISPARQLQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.Minus(s: nodes[0], p: nodes[1], o: nodes[2]);
        }
        /// <summary>
        /// Exists expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Exists<T>(this ISPARQLQueryable<T> source, string s, string p, string o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(s, typeof(string)), Expression.Constant(p), Expression.Constant(o) }));
        }
        /// <summary>
        /// Exists expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Exists<T>(this ISPARQLUnionQueryable<T> source, string s, string p, string o)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Exists<T>(s, p, o);
        }

        /// <summary>
        /// Exists expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Exists<T>(this ISPARQLQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.Exists(s: nodes[0], p: nodes[1], o: nodes[2]);
        }
        /// <summary>
        /// NotExists expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> NotExists<T>(this ISPARQLQueryable<T> source, string s, string p, string o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(s, typeof(string)), Expression.Constant(p), Expression.Constant(o) }));
        }
        /// <summary>
        /// NotExists expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> NotExists<T>(this ISPARQLUnionQueryable<T> source, string s, string p, string o)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).NotExists<T>(s, p, o);
        }

        /// <summary>
        /// NotExists expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> NotExists<T>(this ISPARQLQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.NotExists(s: nodes[0], p: nodes[1], o: nodes[2]);
        }

    }
}
