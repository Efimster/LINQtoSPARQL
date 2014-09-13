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
        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="s">subject</param>
        ///// <param name="p">predicate</param>
        ///// <param name="o">object</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string s, string p, dynamic o)
        //{
        //    return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Match<T>(s, p, o);
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
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, string p, string o)
        //{
        //    return source.Match(s.GetMemberAccessName(), p, o);
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
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string s, Expression<Func<T, dynamic>> p, string o)
        //{
        //    return source.Match<T>(s, p.GetMemberAccessName(), o);
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
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string s, string p, Expression<Func<T, dynamic>> o)
        //{
        //    return source.Match<T>(s, p, o.GetMemberAccessName());
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
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, string o)
        //{
        //    return source.Match<T>(s.GetMemberAccessName(), p.GetMemberAccessName(), o);
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
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, string p, Expression<Func<T, dynamic>> o)
        //{
        //    return source.Match<T>(s.GetMemberAccessName(), p, o.GetMemberAccessName());
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
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        //{
        //    return source.Match<T>(s, p.GetMemberAccessName(), o.GetMemberAccessName());
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
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> s, Expression<Func<T, dynamic>> p, Expression<Func<T, dynamic>> o)
        //{
        //    return Match<T>(source, s.GetMemberAccessName(), p.GetMemberAccessName(), o.GetMemberAccessName());
        //}

        ///// <summary>
        ///// Match expression
        ///// </summary>
        ///// <typeparam name="T">element type</typeparam>
        ///// <param name="source">query</param>
        ///// <param name="triple">triple</param>
        ///// <returns>query</returns>
        //public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string triple)
        //{
        //    var nodes = triple.SplitExt(" ").ToArray();
        //    return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Match(s: nodes[0], p: nodes[1], o: nodes[2]);
        //}



    }
}
