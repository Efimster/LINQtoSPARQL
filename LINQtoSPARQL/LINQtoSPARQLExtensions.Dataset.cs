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
        /// With clause expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="source">iri</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> With<T>(this ISPARQLQueryable<T> source, string iri)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(iri) }));
        }

        /// <summary>
        /// FROM clause expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="source">iri</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> From<T>(this ISPARQLQueryable<T> source, string iri)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(iri) }));
        }

        /// <summary>
        /// FROM NAMED clause expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="source">iri</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> FromNamed<T>(this ISPARQLQueryable<T> source, string iri)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(iri) }));
        }
        /// <summary>
        /// USING clause expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="source">iri</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Using<T>(this ISPARQLQueryable<T> source, string iri)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(iri) }));
        }

        /// <summary>
        /// USING NAMED clause expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="source">iri</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> UsingNamed<T>(this ISPARQLQueryable<T> source, string iri)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(iri) }));
        }

        /// <summary>
        /// Graph expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="source">iri</param>
        /// <returns>query</returns>
        public static ISPARQLQueryable<T> Graph<T>(this ISPARQLQueryable<T> source, string iri)
        {
            if (source == null)
                throw new ArgumentNullException("source");


            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(iri) }));
        }


    }
}
