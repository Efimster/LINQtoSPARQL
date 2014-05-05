using System.Linq;
using Should.Fluent;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class DatasetFixture
    {

        [Theory(DisplayName = "Graph Pattern"), Xunit.Trait("SPARQL Query", "dataset"),
        InlineData(@"
                    @prefix foaf:       <http://xmlns.com/foaf/0.1/> .
                    @prefix rdf:        <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
                    @prefix graph:      <http://example.org/foaf/> .
                    graph:def  {
                        _:b  rdf:type        foaf:Person . }
                    graph:aliceFoaf  {   
                        _:a  rdf:type        foaf:Person .
                        _:a  foaf:name       ""Alice"" 
                    }
                    graph:bobFoaf  {   
                        _:a  rdf:type        foaf:Person .
                        _:a  foaf:name       ""Bob"" 
                    }")]
        public void TestQueryDataset1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, useStore: true, defaultGraphUri: "http://example.org/foaf/def");
            
            var list = query.Match("?s ?p ?o")
                .Select("?s ?p ?o")
                .From("http://example.org/foaf/aliceFoaf")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(2);

            list = query.Graph("graph:aliceFoaf")
                    .Match("?s ?p ?o")
                .End()
                .Select("?s ?p ?o")
                .From("graph:def")
                .FromNamed("graph:aliceFoaf")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .Prefix("graph", "http://example.org/foaf/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(2);

            list = query.Graph("?g")
                    .Match("?s ?p ?o")
                .End()
                .Select("?g ?s ?p ?o")
                .From("graph:def")
                .FromNamed("graph:aliceFoaf")
                .FromNamed("http://example.org/foaf/bobFoaf")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .Prefix("graph", "http://example.org/foaf/")
                .AsEnumerable()
                .ToList();

            list.ToList().Count.Should().Equal(4);
        }

        [Theory(DisplayName = "DELETE DATA"),
           Xunit.Trait("SPARQL Update", "dataset"),
           InlineData(@"
            @prefix dc: <http://purl.org/dc/elements/1.1/> .
            @prefix ns: <http://example.org/ns#> .
                        
            ns:g1{
                <http://example/book2> ns:price 42 .
                <http://example/book2> dc:title ""David Copperfield"" .}
            ns:g2{
                <http://example/book2> dc:creator ""Edmund Wells"" .}
            
        ")]
        public void TestUpdateDataset1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: false, useStore: true, defaultGraphUri: "http://example.org/ns#g2");

            var res = query.Delete()
                    .Graph("ns:g1")
                        .Match(s: "<http://example/book2>", p: "dc:title", o: "\"David Copperfield\"")
                        .And(p:"dc:creator",o:"\"Edmund Wells\"")
                    .End()
                .Prefix("dc", "http://purl.org/dc/elements/1.1/")
                .Prefix("ns", "http://example.org/ns#")
                .ExecuteUpdate();
            res.Should().Equal(0);

            var list = query.Match("?s ?p ?o")
                .Select("?s ?p ?o")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(1);
            list.Any(x => x.p == "creator").Should().Be.True();
        }

        [Theory(DisplayName = "INSERT DATA"),
         Xunit.Trait("SPARQL Update", "dataset"),
         InlineData(@"
                @prefix dc: <http://purl.org/dc/elements/1.1/> .
                @prefix ns: <http://example.org/ns#> .
                ns:g1 {
                <http://example/book1> ns:price 42 .}")]
        public void TestUpdateDataset2(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: false, useStore: true, defaultGraphUri: "http://example.org/ns#g1");
            var res = query.Insert()
                   .Graph("ns:g2")
                       .Match(s: "<http://example/book2>", p: "dc:title", o: "\"David Copperfield\"")
                       .And(p: "dc:creator", o: "\"Edmund Wells\"")
                   .End()
               .Prefix("dc", "http://purl.org/dc/elements/1.1/")
               .Prefix("ns", "http://example.org/ns#")
               .ExecuteUpdate();
            res.Should().Equal(0);

            var list = query.Match("?s ?p ?o")
                .Select("?s ?p ?o")
                .From("http://example.org/ns#g2")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(2);
            list.Where(x => x.p == "title" && x.o == "David Copperfield").Count().Should().Equal(1);
            list.Where(x => x.p == "creator" && x.o == "Edmund Wells").Count().Should().Equal(1);
        }

        [Theory(DisplayName = "DELETE/INSERT"),
        Xunit.Trait("SPARQL Update", "dataset"),
        InlineData(@"
            @prefix foaf:  <http://xmlns.com/foaf/0.1/> .
            @prefix ns: <http://example.org/ns#> .
            ns:g1{
            <http://example/president25> foaf:givenName ""Bill"" .

            <http://example/president25> foaf:familyName ""McKinley"" .
            <http://example/president27> foaf:givenName ""Bill"" .
            <http://example/president27> foaf:familyName ""Taft"" .}
            ns:g2{
            <http://example/president42> foaf:givenName ""Bill"" .
            <http://example/president42> foaf:familyName ""Clinton"" .}")]
        public void TestUpdateDataset3(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: true, useStore: true, defaultGraphUri: "http://example.org/ns#g1");
            var res = query
                .Using("ns:g1")
                .Match("?person foaf:givenName Bill")
                .With("ns:g1")
                    .Delete().Match("?person foaf:givenName Bill")
                    .Insert().Match("?person foaf:givenName William")
               .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
               .Prefix("ns", "http://example.org/ns#")
               .ExecuteUpdate();
            
            res.Should().Equal(0);

            res = query
                .UsingNamed("ns:g2")
                .Graph("?g")
                    .Match("?person foaf:givenName Bill")
                .End()
                .With("ns:g2")
                    .Delete().Match("?person foaf:givenName Bill")
                    .Insert().Match("?person foaf:givenName Ben")
               .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
               .Prefix("ns", "http://example.org/ns#")
               .ExecuteUpdate();

            var list = query.Match("?s ?p ?o")
                .Select("?s ?p ?o")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(4);
            list.Where(x => x.p == "givenName" && x.o == "William").Count().Should().Equal(2);
            list.Where(x => x.p == "givenName" && x.o == "Bill").Count().Should().Equal(0);

            list = query.Match("?s ?p ?o")
                .Select("?s ?p ?o")
                .From("ns:g2")
                .Prefix("ns", "http://example.org/ns#")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(2);
            list.Where(x => x.p == "givenName" && x.o == "Ben").Count().Should().Equal(1);
            list.Where(x => x.p == "givenName" && x.o == "Bill").Count().Should().Equal(0);


        }
    }
}
