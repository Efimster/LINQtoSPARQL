using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSPARQLSpace
{
    /// <summary>
    /// Base interface of SPARQL LINQ query
    /// </summary>
    public interface ISPARQLQueryable
    {
        ISPARQLQueryProvider Provider { get; }
        Expression Expression { get; }
    }
    /// <summary>
    /// Interface of SPARQL query provider
    /// </summary>
    public interface ISPARQLQueryProvider : IQueryProvider
    {
        ISPARQLQueryable<T> CreateSPARQLQuery<T>(Expression expression);
        IEnumerable<T> ExecuteEnumerable<T>(Expression expression);
    }
    /// <summary>
    /// Match query expression interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISPARQLMatchedQueryable<T> : ISPARQLQueryable<T>
    {}

    /// <summary>
    /// Union/Either query expression interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISPARQLUnionQueryable<T> : ISPARQLMatchedQueryable<T>
    {}
    /// <summary>
    /// Typed interface of SPARQL LINQ query
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    public interface ISPARQLQueryable<T> : ISPARQLQueryable { }


}
