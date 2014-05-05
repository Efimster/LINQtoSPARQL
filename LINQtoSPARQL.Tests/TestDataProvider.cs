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
using DynamicSPARQLSpace.dotNetRDF;

namespace LINQtoSPARQLSpace.Tests
{
    public static class TestDataProvider
    {
        public static ISPARQLQueryable<T> GetQuerable<T>(string data, 
            bool autoquotation = true, 
            bool treatUri = true,
            IEnumerable<Prefix> prefixes = null,
            bool skipTriplesWithEmptyObject = false,
            bool mindAsterisk = false,
            bool useStore = false,
            string defaultGraphUri = "http://test.org/defaultgraph")
        {
            DynamicSPARQLSpace.dotNetRDF.Connector connector = null;

            if (useStore)
            {
                var store = new VDS.RDF.TripleStore();
                store.LoadFromString(data);
                connector = new Connector(new InMemoryDataset(store, new Uri(defaultGraphUri)));
            }
            else
            {
                var graph = new VDS.RDF.Graph();
                graph.LoadFromString(data);
                connector = new Connector(new InMemoryDataset(graph));
            }


            dynamic dyno = DynamicSPARQL.CreateDyno(connector.GetQueryingFunction(), 
                updateFunc: connector.GetUpdateFunction(),
                autoquotation: autoquotation,
                treatUri: treatUri,
                prefixes:prefixes,
                skipTriplesWithEmptyObject:skipTriplesWithEmptyObject,
                mindAsterisk:mindAsterisk);

            return new SPARQLQuery<T>(dyno);
        }
    }
}
