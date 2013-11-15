using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSPARQLSpace
{
    public interface ISPARQLQueryable
    {
        ISPARQLQueryProvider Provider { get; }
        Expression Expression { get; }
    }

    public interface ISPARQLQueryProvider : IQueryProvider
    {
        ISPARQLQueryable<T> CreateSPARQLQuery<T>(Expression expression);
        IEnumerable<T> ExecuteEnumerable<T>(Expression expression);
    }

    public interface ISPARQLMatchedQueryable<T> : ISPARQLQueryable<T>
    {}


    public interface ISPARQLUnionQueryable<T> : ISPARQLMatchedQueryable<T>
    {}

    public interface ISPARQLQueryable<T> : ISPARQLQueryable { }


}
