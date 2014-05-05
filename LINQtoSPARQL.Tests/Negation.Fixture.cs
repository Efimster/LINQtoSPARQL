using System.Linq;
using Should.Fluent;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class NegationFixture
    {
        [Theory(DisplayName = "Removing Possible Solutions(MINUS)"), Xunit.Trait("SPARQL Query", ""),
        InlineData(@"@prefix :       <http://example/> .
                @prefix foaf:   <http://xmlns.com/foaf/0.1/> .

                :alice  foaf:givenName ""Alice"" ;
                        foaf:familyName ""Smith"" .

                :bob    foaf:givenName ""Bob"" ;
                        foaf:familyName ""Jones"" .

                :carol  foaf:givenName ""Carol"" ;
                        foaf:familyName ""Smith"" .")]
        public void TestMinus1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, treatUri: true);
            var list = query.Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .Select("?s")
                .Distinct()
                .Match("?s ?p ?o")
                .Minus("?s foaf:givenName Bob")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(2);
            list.Where(x => x.s == "alice").Count().Should().Equal(1);
            list.Where(x => x.s == "carol").Count().Should().Equal(1);

        }

        [Theory(DisplayName = "Presence of a Pattern(FILTER EXISTS)"), Xunit.Trait("SPARQL Query", ""),
         InlineData(@"@prefix  :       <http://example/> .
                @prefix  rdf:    <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
                @prefix  foaf:   <http://xmlns.com/foaf/0.1/> .

                :alice  rdf:type   foaf:Person .
                :alice  foaf:name  ""Alice"" .
                :bob    rdf:type   foaf:Person . ")]
        public void TestExists(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, treatUri: true);
            var list = query.Match("?person rdf:type  foaf:Person")
                .Exists("?person foaf:name ?name")
                .Select("?person")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .Prefix("rdf:", "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(1);
            list.Where(x => x.person == "alice").Count().Should().Equal(1);
        }

        [Theory(DisplayName = "Absence of a Pattern(FILTER NOT EXISTS)"), Xunit.Trait("SPARQL Query", ""),
         InlineData(@"@prefix  :       <http://example/> .
                @prefix  rdf:    <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .
                @prefix  foaf:   <http://xmlns.com/foaf/0.1/> .

                :alice  rdf:type   foaf:Person .
                :alice  foaf:name  ""Alice"" .
                :bob    rdf:type   foaf:Person . ")]
        public void TestNotExists(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, treatUri: true);
            var list = query.Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .Prefix("rdf:", "http://www.w3.org/1999/02/22-rdf-syntax-ns#")
                .Select("?person")
                .Match("?person rdf:type  foaf:Person")
                .NotExists("?person foaf:name ?name")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(1);
            list.Where(x => x.person == "bob").Count().Should().Equal(1);

        }
    }
}
