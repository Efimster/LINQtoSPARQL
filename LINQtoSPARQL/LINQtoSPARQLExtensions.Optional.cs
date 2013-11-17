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
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string S, string P, string O)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(S, typeof(string)), Expression.Constant(P), Expression.Constant(O) }));
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Optional<T>(this ISPARQLUnionQueryable<T> source, string S, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Optional<T>(S, P, O);
        }

        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.Optional(S: nodes[0], P: nodes[1], O: nodes[2]);
        }
    }
}
