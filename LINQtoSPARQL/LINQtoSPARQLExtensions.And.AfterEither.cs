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

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string S, string P, string O)
        {
            return source.Match<T>(S, P, O);
        }


        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(P, O);
        }


        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string O)
        {
            return (ISPARQLUnionQueryable<T>)source.And_1<T>(O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, string P, string O)
        {
            return source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string S, Expression<Func<T, dynamic>> P, string O)
        {
            return source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string S, string P, Expression<Func<T, dynamic>> O)
        {
            return source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, Expression<Func<T, dynamic>> P, string O)
        {
            return source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, string P, Expression<Func<T, dynamic>> O)
        {
            return source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string S, Expression<Func<T, dynamic>> P, Expression<Func<T, dynamic>> O)
        {
            return source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, Expression<Func<T, dynamic>> P, Expression<Func<T, dynamic>> O)
        {
            return source.Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> P, string O)
        {
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(pName, O);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> P, Expression<Func<T, dynamic>> O)
        {
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(pName, oName);
        }

        public static ISPARQLUnionQueryable<T> And<T>(this ISPARQLUnionQueryable<T> source, string P, Expression<Func<T, dynamic>> O)
        {
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            return (ISPARQLUnionQueryable<T>)source.And_2<T>(P, oName);
        }

    }
}
