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
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string s, string p, dynamic o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(s, typeof(string)), Expression.Constant(p), Expression.Constant(o) }));
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, string p, dynamic o)
        {
            return Optional(source, s.GetMemberAccessName(), p, o);
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string s, Expression<Func<T, dynamic>> p, dynamic o)
        {
            return Optional<T>(source, s, p.GetMemberAccessName(), o);
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string s, string p, Expression<Func<T, dynamic>> o)
        {
            return source.Optional<T>(s, p, o.GetMemberAccessName());
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, dynamic o)
        {
            return Optional<T>(source, s.GetMemberAccessName(), p.GetMemberAccessName(), o);
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <param name="o"></param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, string p, Expression<Func<T, dynamic>> o)
        {
            return source.Optional<T>(s.GetMemberAccessName(), p, o.GetMemberAccessName());
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            return source.Optional<T>(s, p.GetMemberAccessName(), o.GetMemberAccessName());
        }
        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            return source.Optional<T>(s.GetMemberAccessName(), p.GetMemberAccessName(), o.GetMemberAccessName());
        }

        ///// <summary>
        ///// Optional expression
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> Optional<T>(this ISPARQLUnionQueryable<T> source, string s, string p, dynamic o)
        //{
        //    return (ISPARQLUnionQueryable<T>)Optional<T>((ISPARQLQueryable<T>)source, s, p, o);
        //}

        /// <summary>
        /// Optional expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchQueryable<T> Optional<T>(this ISPARQLQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.Optional(s: nodes[0], p: nodes[1], o: nodes[2]);
        }

        private static string GetMemberAccessName<T>(this Expression<Func<T, dynamic>> expression)
        {
            MemberExpression member = (MemberExpression)(expression.Body as UnaryExpression != null ? 
                ((UnaryExpression)expression.Body).Operand 
                : expression.Body);
            return "?" + member.Member.Name.ToLower();
        }
    }
}
