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
        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, string P, string O)
        {
            return (ISPARQLUnionQueryable<T>)((ISPARQLQueryable<T>)source).Match<T>(S, P, O);
        }

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, string P, string O)
        {
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return source.Match(sName, P, O);
        }

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, Expression<Func<T, dynamic>> P, string O)
        {
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            return source.Match<T>(S, pName, O);
        }

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, string P, Expression<Func<T, dynamic>> O)
        {
            string name = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            return source.Match<T>(S, P, name);
        }

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, Expression<Func<T, dynamic>> P, string O)
        {
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return source.Match<T>(sName, pName, O);
        }

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, string P, Expression<Func<T, dynamic>> O)
        {
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return source.Match<T>(sName, P, oName);
        }

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, string S, Expression<Func<T, dynamic>> P, Expression<Func<T, dynamic>> O)
        {
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            return source.Match<T>(S, pName, oName);
        }

        public static ISPARQLUnionQueryable<T> Match<T>(this ISPARQLUnionQueryable<T> source, Expression<Func<T, dynamic>> S, Expression<Func<T, dynamic>> P, Expression<Func<T, dynamic>> O)
        {
            string oName = "?" + ((MemberExpression)O.Body).Member.Name.ToLower();
            string pName = "?" + ((MemberExpression)P.Body).Member.Name.ToLower();
            string sName = "?" + ((MemberExpression)S.Body).Member.Name.ToLower();
            return Match<T>(source, sName, pName, oName);
        }



    }
}
