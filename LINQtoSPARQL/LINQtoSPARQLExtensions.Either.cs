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
        /// Either expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Either<T>(this ISPARQLQueryable<T> source, string s, string p, dynamic o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(s, typeof(string)), Expression.Constant(p), Expression.Constant(o) }));
        }

        /// <summary>
        /// Either expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Either<T>(this ISPARQLQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.Either(s: nodes[0], p: nodes[1], o: nodes[2]);
        }

        /// <summary>
        /// Or expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Or<T>(this ISPARQLMatchQueryable<T> source, string s, string p, dynamic o)
        {
            return (ISPARQLMatchQueryable<T>)Or<T>((ISPARQLQueryable)source, s, p, o);
        }
        /// <summary>
        /// Or expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        private static ISPARQLQueryable<T> Or<T>(this ISPARQLQueryable source, string s, string p, dynamic o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(s, typeof(string)), Expression.Constant(p), Expression.Constant(o) }));
        }

        /// <summary>
        /// Or expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Or<T>(this ISPARQLMatchQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.Or(s: nodes[0], p: nodes[1], o: nodes[2]);
        }
    }
}
