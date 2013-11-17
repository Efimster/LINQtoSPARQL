using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSPARQLSpace
{
    public static partial class LINQtoSPARQLExtensions
    {
        /// <summary>
        /// Filter expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ISPARQLMatchedQueryable<T> FilterBy<T>(this ISPARQLQueryable<T> source, string filter)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(filter, typeof(string)) }));
        }

        public static ISPARQLUnionQueryable<T> FilterBy<T>(this ISPARQLUnionQueryable<T> source, string filter)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).FilterBy<T>(filter);
        }
    }
}
