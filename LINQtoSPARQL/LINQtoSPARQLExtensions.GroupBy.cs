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
    }
}
