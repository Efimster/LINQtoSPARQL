using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DynamicSPARQLSpace;
using HelperExtensionsLibrary.Strings;

namespace LINQtoSPARQLSpace
{
    public class SPARQLQueryProvider : IQueryProvider, ISPARQLQueryProvider
    {
        private DynamicSPARQL Dyno { get; set; }
        public SPARQLQueryProvider(DynamicSPARQL dynoSPARQL)
        {
            Dyno = dynoSPARQL;
        }

        IQueryable<S> IQueryProvider.CreateQuery<S>(Expression expression)
        {
            return new SPARQLQueryAsQueryable<S>(this, expression);
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotSupportedException();
        }

        S IQueryProvider.Execute<S>(Expression expression)
        {
            return (S)this.Execute(expression);
        }

        public object Execute(Expression expression)
        {
            return ExecuteEnumerable<dynamic>(expression);
        }

        public IEnumerable<T> ExecuteEnumerable<T>(Expression expression)
        {
            var translator = this.Translate(expression);

            foreach(var res in Dyno.Select<T>(
                   prefixes: translator.Prefixes,
                    projection: translator.SelectClause,
                    where: translator.WhereClause,
                    groupBy:translator.GroupByClause,
                    having:translator.HavingClause,
                    orderBy: translator.OrderByClause,
                    limit: translator.LimitClause.ToString(),
                    offset: translator.OffsetClause.ToString())
                )
            {
                yield return res;
            }
        }

        public string GetQueryText(Expression expression)
        {
            var translator = this.Translate(expression);
            
            string query = new string[] { translator.Prefixes.GetPrefixesString(),
                translator.SelectClause,
                translator.WhereClause.ToString(),
                translator.GroupByClause,
                translator.HavingClause, 
                translator.OrderByClause,
                translator.LimitClause.ToString(),
                translator.OffsetClause.ToString()}.Join2String(Environment.NewLine);
            
            return query;
        }

        private SPARQLQueryTranslator Translate(Expression expression)
        {
            var translator = new SPARQLQueryTranslator();
            translator.Translate(expression);
            return translator;
        }


        public ISPARQLQueryable<T> CreateSPARQLQuery<T>(Expression expression)
        {
            return new SPARQLQueryInternal<T>(this, expression);
        }
    }
}
