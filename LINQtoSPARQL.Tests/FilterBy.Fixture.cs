﻿using System;
using System.Linq;
using DynamicSPARQLSpace;
using Should.Fluent;
using VDS.RDF;
using VDS.RDF.Query;
using Xunit.Extensions;


namespace LINQtoSPARQLSpace.Tests
{
    public class FilterByFixture
    {

        private static ISPARQLQueryable<T> GetQuerable<T>(string data, bool autoquotation = true, bool treatUri = true)
        {
            var graph = new Graph();
            graph.LoadFromString(data);

            Func<string, SparqlResultSet> sendSPARQLQuery = xquery => graph.ExecuteQuery(xquery) as SparqlResultSet;
            dynamic dyno = DynamicSPARQL.CreateDyno(sendSPARQLQuery, autoquotation, treatUri);

            return new SPARQLQuery<T>(dyno);
        }

        [Theory(DisplayName = "Filtering(Restricting the Value of Strings) "),
        InlineData(@"@prefix dc:   <http://purl.org/dc/elements/1.1/> .
                @prefix :     <http://example.org/book/> .
                @prefix ns:   <http://example.org/ns#> .

                :book1  dc:title  ""SPARQL Tutorial"" .
                :book1  ns:price  42 .
                :book2  dc:title  ""The Semantic Web"" .
                :book2  ns:price  23 .")]
        public void TestFilter1(string data)
        {
            var query = GetQuerable<dynamic>(data, autoquotation: false);
            var list = query.Match(s: "?x", p: "dc:title", o: "?title").FilterBy("regex(?title, \"^SPARQL\")")
                .Select("?title")
                .Prefix("dc:", "http://purl.org/dc/elements/1.1/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(1);
            list.Any(x => x.title == "SPARQL Tutorial").Should().Be.True();

            query = GetQuerable<dynamic>(data, autoquotation: true);
            list = query.Match(s: "?x", p: "dc:title", o: "?title").FilterBy("regex(?title, web, i )")
                .Select("?title")
                .Prefix("dc:", "http://purl.org/dc/elements/1.1/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(1);
            list.Any(x => x.title == "The Semantic Web").Should().Be.True();

        }
    }
}
