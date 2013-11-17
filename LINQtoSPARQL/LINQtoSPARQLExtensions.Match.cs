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
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, string s, string p, string o)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return (ISPARQLMatchedQueryable<T>)source.Provider.CreateSPARQLQuery<T>(Expression.Call(null, ((MethodInfo) MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(T) }),
                new Expression[] { source.Expression, Expression.Constant(s, typeof(string)), Expression.Constant(p), Expression.Constant(o) }));
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
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, string p, string o)
        {
            string sName = "?" + ((MemberExpression)s.Body).Member.Name.ToLower();
            return source.Match(sName, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, string s, Expression<Func<T, dynamic>> p, string o)
        {
            string pName = "?" + ((MemberExpression)p.Body).Member.Name.ToLower();
            return source.Match<T>(s, pName, o);
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
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, string s, string p, Expression<Func<T, dynamic>> o)
        {
            string name = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            return source.Match<T>(s, p, name);
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
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, string o)
        {
            string pName = "?" + ((MemberExpression)p.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)s.Body).Member.Name.ToLower();
            return source.Match<T>(sName, pName, o);
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
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, string p, Expression<Func<T, dynamic>> o)
        {
            string oName = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)s.Body).Member.Name.ToLower();
            return source.Match<T>(sName, p, oName);
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
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, string s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            string oName = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            string pName = "?" + ((MemberExpression)p.Body).Member.Name.ToLower();
            return source.Match<T>(s, pName, oName);
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
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            string oName = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            string pName = "?" + ((MemberExpression)p.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)s.Body).Member.Name.ToLower();
            return source.Match<T>(sName, pName, oName);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLMatchedQueryable<T> Match<T>(this ISPARQLQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return source.Match(s: nodes[0], p: nodes[1], o: nodes[2]);
        }



    }
}
