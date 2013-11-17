using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, string p, string o)
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string p, string o)
        {
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(p, o);
        }

        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string o)
        {
            return (ISPARQLUnionQueryable<T>)source.And_1<T>(o);
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, string p, string o)
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, Expression<Func<T, dynamic>> p, string o)
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, string p, Expression<Func<T, dynamic>> o)
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, string o)
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, string p, Expression<Func<T, dynamic>> o)
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
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
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            return source.Match<T>(s, p, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="P">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> P, string o)
        {
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(pName, o);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        {
            string pName = "?" + ((MemberExpression)p.Body).Member.Name.ToLower();
            string oName = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(pName, oName);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string p, Expression<Func<T, dynamic>> o)
        {
            string oName = "?" + ((MemberExpression)o.Body).Member.Name.ToLower();
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(p, oName);
        }

    }
}
