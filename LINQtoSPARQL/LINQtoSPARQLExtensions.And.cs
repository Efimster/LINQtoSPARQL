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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, string s, string p, dynamic o)
        {
            return Match<T>(source, s, p, o);
        }

        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, string p, dynamic o)
        {
            return (ISPARQLMatchQueryable<T>)And_2<T>(source, p, o);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        private static ISPARQLQueryable<T> And_2<T>(this ISPARQLQueryable<T> source, string p, dynamic o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(p), Expression.Constant(o, typeof(object)) }));
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="o">object or triple or partional triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, dynamic o)
        {
            if (o as string != null)
            {
                IList<string> obj = ((string)o).SplitExt(" ").ToArray();
                if (obj.Count == 3)
                    return Match(source, s: obj[0], p: obj[1], o: obj[2]);

                if (obj.Count == 2)
                    return (ISPARQLMatchQueryable<T>)And_2<T>(source, p: obj[0], o: obj[1]);
            }
            
            
            
            return (ISPARQLMatchQueryable<T>)And_1<T>(source, o);
        }

        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        private static ISPARQLQueryable<T> And_1<T>(this ISPARQLQueryable<T> source, dynamic o)
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, Expression<Func<T, dynamic>> s, string p, dynamic o)
        {
            return Match<T>(source , s, p, o);
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, string s, Expression<Func<T, dynamic>> p, dynamic o)
        {
            return Match<T>(source, s, p, o);
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, string s, string p, Expression<Func<T, dynamic>> o)
        {
            return Match<T>(source, s, p, o);
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, dynamic o)
        {
            return Match<T>(source, s, p, o);
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, Expression<Func<T, dynamic>> s, string p, Expression<Func<T, dynamic>> o)
        {
            return Match<T>(source, s, p, o);
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, string s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
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
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, Expression<Func<T, dynamic>> p, dynamic o)
        {
            return (ISPARQLMatchQueryable<T>)And_2<T>(source, p.GetMemberAccessName(), o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            return (ISPARQLMatchQueryable<T>)source.And_2<T>(p.GetMemberAccessName(), o.GetMemberAccessName());
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> And<T>(this ISPARQLMatchQueryable<T> source, string p, Expression<Func<T, dynamic>> o)
        {
            return (ISPARQLMatchQueryable<T>)source.And_2<T>(p, o.GetMemberAccessName());
        }

    }
}
