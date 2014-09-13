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
                    offset: translator.OffsetClause.ToString(),
                    from: translator.FromClause,
                    fromNamed:translator.FromNamedClause)
                )
            {
                yield return res;
            }
        }

        public object ExecuteUpdate(Expression expression)
        {
            var translator = this.Translate(expression);

            if (translator.InsertClause == null)
                return Dyno.Delete(prefixes: translator.Prefixes, where: translator.WhereClause, delete: translator.DeleteClause);
            else if (translator.DeleteClause == null)
                return Dyno.Insert(prefixes: translator.Prefixes, where: translator.WhereClause, insert: translator.InsertClause);
            else
                return Dyno.Update(prefixes: translator.Prefixes, where: translator.WhereClause, 
                    delete: translator.DeleteClause, insert: translator.InsertClause,
                    with:translator.WithClause, usingClause:translator.UsingClause, usingNamed:translator.UsingNamedClause);
        }

        public string GetQueryText(Expression expression)
        {
            var translator = this.Translate(expression);
            IEnumerable<string> parts = new []
            {
                    Prefix.Collection2String(Dyno.Prefixes.Concat(translator.Prefixes)),  
                    translator.WithClause != null ? translator.WithClause.ToString() : null,
                    translator.DeleteClause != null ? "DELETE " 
                                + translator.DeleteClause.ToString(Dyno.AutoQuotation, Dyno.SkipTriplesWithEmptyObject, false) 
                            : string.Empty,
                    translator.InsertClause != null ? "INSERT " 
                                + translator.InsertClause.ToString(Dyno.AutoQuotation, Dyno.SkipTriplesWithEmptyObject, false) 
                            : string.Empty,
                    translator.UsingClause != null ? translator.UsingClause.ToString() : null,
                    UsingNamed.Collection2String(translator.UsingNamedClause),
                    !string.IsNullOrEmpty(translator.SelectClause) ? "SELECT " + translator.SelectClause : string.Empty,
                    translator.FromClause != null ? translator.FromClause.ToString() : null,
                    FromNamed.Collection2String(translator.FromNamedClause),
                    translator.WhereClause != null ? "WHERE "
                                + translator.WhereClause.ToString(Dyno.AutoQuotation, Dyno.SkipTriplesWithEmptyObject, false)
                            : string.Empty,

                    translator.GroupByClause,
                    translator.HavingClause, 
                    translator.OrderByClause,
                    translator.LimitClause.ToString(),
                    translator.OffsetClause.ToString()
            };
            
            string query = string.Join(Environment.NewLine, parts.Where(part => !string.IsNullOrEmpty(part)));
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

        public string LastQueryPrint 
        {
            get { return Dyno.LastQueryPrint; } 
        }


        //public ISPARQLQueryable Merge(Expression expression, ISPARQLQueryable otherQuery)
        //{
        //    var translator1 = this.Translate(otherQuery.Expression);
        //    var translator2 = this.Translate(otherQuery.Expression);

        //    var prefixes = translator1.Prefixes.Union(translator2.Prefixes);
        //    var deleteClause = MergeGroups(translator1.DeleteClause, translator2.DeleteClause);

        //    return null;
        //}

        //private Group MergeGroups(Group group1, Group group2)
        //{
        //    return SPARQL.Group(group1.Items.Union(group2.Items).ToArray());
        //}

        public ISPARQLQueryable<T> Merge<T>(ISPARQLQueryable<T> baseQuery, ISPARQLQueryable<T> otherQuery)
        {
            var translator = new SPARQLMergeTranslator();

            return (ISPARQLMatchQueryable<T>)CreateSPARQLQuery<T>(translator.Merge(baseQuery.Expression, otherQuery.Expression));
        
        }
    }
}
