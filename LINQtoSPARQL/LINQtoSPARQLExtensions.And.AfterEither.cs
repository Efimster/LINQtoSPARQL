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
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, string p, dynamic o)
        //{
        //    return Match<T>(source, s, p, o);
        //}

        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string p, dynamic o)
        //{
        //    return (ISPARQLUnionQueryable<T>)And_2<T>(source, p, o);
        //}

        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, dynamic o)
        //{
        //    return (ISPARQLUnionQueryable<T>)And_1<T>(source, o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, string p, dynamic o)
        //{
        //    return Match<T>(source, s, p, o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, Expression<Func<T, dynamic>> p, dynamic o)
        //{
        //    return Match<T>(source, s, p, o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, string p, Expression<Func<T, dynamic>> o)
        //{
        //    return source.Match<T>(s, p, o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, dynamic o)
        //{
        //    return Match<T>(source, s, p, o);
        //}
        /// <summary>
        /// Match expression
        /// </summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="source">query</param>
        /// <param name="s">subject</param>
        /// <param name="p">predicate</param>
        /// <param name="o">object</param>
        /// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, string p, Expression<Func<T, dynamic>> o)
        //{
        //    return source.Match<T>(s, p, o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        //{
        //    return source.Match<T>(s, p, o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        //{
        //    return source.Match<T>(s, p, o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> p, dynamic o)
        //{
        //    return (ISPARQLUnionQueryable<T>)And_2<T>(source, p.GetMemberAccessName(), o);
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        //{
        //    return (ISPARQLUnionQueryable<T>)source.And_2<T>(p.GetMemberAccessName(), o.GetMemberAccessName());
        //}
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string p, Expression<Func<T, dynamic>> o)
        //{
        //    return (ISPARQLUnionQueryable<T>)source.And_2<T>(p, o.GetMemberAccessName());
        //}

    }
}
