using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSPARQLSpace;
using VDS.RDF;
using VDS.RDF.Query;
using System.Xml;
using VDS.RDF.Update;
using VDS.RDF.Parsing;
using VDS.RDF.Query.Datasets;

namespace LINQtoSPARQLSpace.Tests
{
    public static class TestDataProvider
    {
        public static ISPARQLQueryable<T> GetQuerable<T>(string data, 
            bool autoquotation = true, 
            bool treatUri = true,
            IEnumerable<Prefix> prefixes = null,
            bool skipTriplesWithEmptyObject = false,
            bool mindAsterisk = false)
        {
            var graph = new Graph();
            graph.LoadFromString(data);
            var processor = new LeviathanUpdateProcessor(new InMemoryDataset(graph));



            Func<string, SparqlResultSet> sendSPARQLQuery =
                xquery => graph.ExecuteQuery(xquery) as SparqlResultSet;
            Func<string, object> updateSPARQL = uquery =>
            {
                processor.ProcessCommandSet(new SparqlUpdateParser().ParseFromString(uquery));
                return 0;
            };

            dynamic dyno = DynamicSPARQL.CreateDyno(sendSPARQLQuery,
                updateFunc: updateSPARQL,
                autoquotation: autoquotation,
                treatUri: treatUri,
                prefixes:prefixes,
                skipTriplesWithEmptyObject:skipTriplesWithEmptyObject,
                mindAsterisk:mindAsterisk);

            return new SPARQLQuery<T>(dyno);
        }
    }
}
