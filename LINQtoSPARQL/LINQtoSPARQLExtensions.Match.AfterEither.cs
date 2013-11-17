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
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Match<T>(S, P, O);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, string P, string O)
        {
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return source.Match(sName, P, O);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, Expression<Func<T, dynamic>> P, string O)
        {
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            return source.Match<T>(S, pName, O);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, string P, Expression<Func<T, dynamic>> O)
        {
            string name = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            return source.Match<T>(S, P, name);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, Expression<Func<T, dynamic>> P, string O)
        {
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return source.Match<T>(sName, pName, O);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, string P, Expression<Func<T, dynamic>> O)
        {
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return source.Match<T>(sName, P, oName);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, Expression<Func<T, dynamic>> P, Expression<Func<T, dynamic>> O)
        {
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            return source.Match<T>(S, pName, oName);
        }
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="S">subject</param>
        /// <param name="P">predicate</param>
        /// <param name="O">object</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, Expression<Func<T, dynamic>> P, Expression<Func<T, dynamic>> O)
        {
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return Match<T>(source, sName, pName, oName);
        }

        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="triple">triple</param>
        /// <returns>query</returns>
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string triple)
        {
            var nodes = triple.SplitExt(" ").ToArray();
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Match(S: nodes[0], P: nodes[1], O: nodes[2]);
        }



    }
}
