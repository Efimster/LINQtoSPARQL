using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LINQtoSPARQLSpace
{
    /// <summary>
    /// Interface of SPARQL query provider
    /// </summary>
    public interface ISPARQLQueryProvider : IQueryProvider
    {
        /// <summary>
        /// Constructs SPARQL query
        /// </summary>
        /// <typeparam name="T">type of element</typeparam>
        /// <param name="expression">query expression</param>
        /// <returns></returns>
        ISPARQLQueryable<T> CreateSPARQLQuery<T>(Expression expression);
        /// <summary>
        /// Executes SPARQL query. IEnumerable result expected.
        /// </summary>
        /// <typeparam name="T">type of element</typeparam>
        /// <param name="expression">query expression</param>
        /// <returns>enumerable</returns>
        IEnumerable<T> ExecuteEnumerable<T>(Expression expression);
        /// <summary>
        /// Executes SPARQL query. Scalar result expected.
        /// </summary>
        /// <param name="expression">query expression</param>
        /// <returns>result object</returns>
        object ExecuteUpdate(Expression expression);
    }
}
