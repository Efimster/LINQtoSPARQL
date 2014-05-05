using System.Linq;
using Should.Fluent;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class OptionalFixture
    {

        [Theory(DisplayName = "Optional Pattern Matching"), Xunit.Trait("SPARQL Query", ""),
        InlineData(@"@prefix foaf:       <http://xmlns.com/foaf/0.1/> .
                    @prefix rdf:        <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .

                    _:a  rdf:type        foaf:Person .
                    _:a  foaf:name       ""Alice"" .
                    _:a  foaf:mbox       <mailto:alice@example.com> .
                    _:a  foaf:mbox       <mailto:alice@work.example> .

                    _:b  rdf:type        foaf:Person .
                    _:b  foaf:name       ""Bob"" .")]
        public void TestOptional1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data);

            var list = query.Match(s: "?x", p: "foaf:name", o: "?name").Optional(s: "?x", p: "foaf:mbox", o: "?mbox")
                .Select("?name ?mbox")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(3);

            list.Where(x => x.name == "Alice" && x.mbox == "mailto:alice@example.com").Count().Should().Equal(1);
            list.Where(x => x.name == "Alice" && x.mbox == "mailto:alice@work.example").Count().Should().Equal(1);
            list.Where(x => x.name == "Bob" && x.mbox == null).Count().Should().Equal(1);

        }

        [Theory(DisplayName = "Constraints in Optional Pattern Matching"), Xunit.Trait("SPARQL Query", ""),
             InlineData(@"@prefix dc:   <http://purl.org/dc/elements/1.1/> .
            @prefix :     <http://example.org/book/> .
            @prefix ns:   <http://example.org/ns#> .

            :book1  dc:title  ""SPARQL Tutorial"" .
            :book1  ns:price  42 .
            :book2  dc:title  ""The Semantic Web"" .
            :book2  ns:price  23 .")]
        public void TestOptional2(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data);

            var list = query.Match("?x dc:title ?title").Optional("?x ns:price ?price").FilterBy("?price < 30").End()
                .Select("?title ?price")
                .Prefix("dc:", "http://purl.org/dc/elements/1.1/")
                .Prefix("ns:", "http://example.org/ns#")
                .AsEnumerable()
                .ToList();


            list.Count.Should().Equal(2);

            list.Where(x => x.title == "SPARQL Tutorial" && x.price == null).Count().Should().Equal(1);
            list.Where(x => x.title == "The Semantic Web" && x.price == 23).Count().Should().Equal(1);

        }

        [Theory(DisplayName = "Multiple Optional Graph Patterns"), Xunit.Trait("SPARQL Query", ""),
            InlineData(@"@prefix foaf:  <http://xmlns.com/foaf/0.1/> .
            _:a  foaf:name       ""Alice"" .
            _:a  foaf:homepage   ""http://work.example.org/alice/"" .

            _:b  foaf:name       ""Bob"" .
            _:b  foaf:mbox       <mailto:bob@work.example> .")]
        public void TestOptional3(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data);

            var list = query.Match("?x foaf:name  ?name").Optional("?x foaf:mbox ?mbox").Optional("?x foaf:homepage ?hpage")
               .Select("?name ?mbox ?hpage")
               .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
               .AsEnumerable()
               .ToList();

            list.Count.Should().Equal(2);

            list.Where(x => x.name == "Alice" && x.mbox == null && x.hpage == "http://work.example.org/alice/").Count().Should().Equal(1);
            list.Where(x => x.name == "Bob" && x.mbox == "mailto:bob@work.example" && x.hpage == null).Count().Should().Equal(1);

        }
    }
}
