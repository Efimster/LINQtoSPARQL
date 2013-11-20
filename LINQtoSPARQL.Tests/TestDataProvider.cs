using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSPARQLSpace;
using VDS.RDF;
using VDS.RDF.Query;
using System.Xml;

namespace LINQtoSPARQLSpace.Tests
{
    public static class TestDataProvider
    {
        public static ISPARQLQueryable<T> GetQuerable<T>(string data, bool autoquotation = true, bool treatUri = true)
        {
            var graph = new Graph();
            graph.LoadFromString(data);

            Func<string, SparqlResultSet> queringFunction = xquery => {
                return graph.ExecuteQuery(xquery) as SparqlResultSet;
                
            }; 
            dynamic dyno = DynamicSPARQL.CreateDyno(queringFunction, autoquotation, treatUri);
            return new SPARQLQuery<T>(dyno);
        }
    }
}
