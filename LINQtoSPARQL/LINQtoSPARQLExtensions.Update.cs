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
        /// Delete expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Delete<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression }));
        }

        /// <summary>
        /// Insert expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Insert<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression }));
        }
        /// <summary>
        /// Executes SPARQL Update query
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <returns>result object</returns>
        public static object ExecuteUpdate<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.ExecuteUpdate(source.Expression);

        }

    }
}
