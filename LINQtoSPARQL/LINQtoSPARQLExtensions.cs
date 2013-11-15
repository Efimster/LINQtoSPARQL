using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;


namespace LINQtoSPARQLSpace
{
    public static partial class LINQtoSPARQLExtensions
    {

        public static ISPARQLQueryable<T> End<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression}));
        }



        public static ISPARQLMatchedQueryable<T> FilterBy<T>(this ISPARQLQueryable<T> source, string filter)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(filter, typeof(string))}));
        }



        public static ISPARQLMatchedQueryable<T> Group<T>(this ISPARQLQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression }));
        }

        public static IEnumerable<dynamic> Execute(this ISPARQLQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (IEnumerable<dynamic>)source.Provider.Execute(source.Expression);
        }

        #region intellisense helpers

        #region ISPARQLUnionQueryable

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string S, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string O)
        {
            return (ISPARQLUnionQueryable<T>)source.And_1<T>(O);
        }

        public static ISPARQLUnionQueryable<T> FilterBy<T>(this ISPARQLUnionQueryable<T> source, string filter)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).FilterBy<T>(filter);
        }

        public static ISPARQLUnionQueryable<T> Optional<T>(this ISPARQLUnionQueryable<T> source, string S, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Optional<T>(S, P, O);
        }

        #endregion


        #endregion

        public static ISPARQLQueryable<T> Select<T>(this ISPARQLQueryable<T> source, params string[] items)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            

                return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                    new Expression[] { source.Expression, Expression.Constant(items, typeof(string[]))}));
        }

        public static ISPARQLQueryable<T> Select<T>(this ISPARQLQueryable<T> source)
        {
            return source.Select(typeof(T));
        }

        private static ISPARQLQueryable<T> Select<T>(this ISPARQLQueryable<T> source, Type type)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(type) }));
        }


        public static ISPARQLQueryable<T> OrderBy<T>(this ISPARQLQueryable<T> source, string orderBy)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(orderBy)}));
        }

        public static ISPARQLQueryable<T> Limit<T>(this ISPARQLQueryable<T> source, int number)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(number) }));
        }


        public static IEnumerable<T> AsEnumerable<T>(this ISPARQLQueryable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (IEnumerable<T>)source.Provider.ExecuteEnumerable<T>(source.Expression);
        }


    }


}
