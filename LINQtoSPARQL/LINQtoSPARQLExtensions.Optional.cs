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

        public static ISPARQLMatchedQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string S, string P, string O)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(S, typeof(string)), Expression.Constant(P), Expression.Constant(O) }));
        }
    }
}
