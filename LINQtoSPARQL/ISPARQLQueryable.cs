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
        /// <summary>
        /// Print of last query
        /// </summary>
        string LastQueryPrint { get; }
    }

    /// <summary>
    /// Match query expression interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISPARQLMatchQueryable<T> : ISPARQLQueryable<T> { }
    ///// <summary>
    ///// Union/Either query expression interface
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public interface ISPARQLMatchQueryable<T> : ISPARQLQueryable<T> {}
    /// <summary>
    /// Typed interface of SPARQL LINQ query
    /// </summary>
    /// <typeparam name="T">element type</typeparam>
    public interface ISPARQLQueryable<T> : ISPARQLQueryable 
    {
        /// <summary>
        /// Merges two queries
        /// </summary>
        /// <param name="otherQuery">other query to merge</param>
        /// <returns>resulting query</returns>
        ISPARQLQueryable<T> Merge(ISPARQLQueryable<T> otherQuery);
    }
    /// <summary>
    /// Bind query expression interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISPARQLBindingQueryable<T> : ISPARQLQueryable<T> { }


}
