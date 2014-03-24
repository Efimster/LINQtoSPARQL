using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions;
using Should.Fluent;

namespace LINQtoSPARQLSpace.Tests
{
    public class SelectFixture
    {
        [Theory(DisplayName = "Typed query"), Xunit.Trait("SPARQL Query", ""),
        InlineData(@"@prefix foaf:       <http://xmlns.com/foaf/0.1/> .
                    @prefix rdf:        <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .

                    _:a  rdf:type        foaf:Person .
                    _:a  foaf:name       ""Alice"" .
                    _:a  foaf:mbox       <mailto:alice@example.com> .
                    _:a  foaf:mbox       <mailto:alice@work.example> .

                    _:b  rdf:type        foaf:Person .
                    _:b  foaf:name       ""Bob"" .
                    _:b  foaf:rating     5 .")]
        public void TestProjection(string data)
        {
            var query = TestDataProvider.GetQuerable<Person>(data);

            var list = query.Match(s: "?x", p: "foaf:name", o: o => o.Name)
                .Optional(s: "?x", p: "foaf:mbox", o: o => o.MBox)
                .Optional(s: "?x", p: "foaf:rating", o: o => o.Rating).And(p: "foaf:rating", o: 5).End()
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(3);

            list.Where(x => x.Name == "Alice" && x.MBox == "mailto:alice@example.com").Count().Should().Equal(1);
            list.Where(x => x.Name == "Alice" && x.MBox == "mailto:alice@work.example").Count().Should().Equal(1);
            list.Where(x => x.Name == "Bob" && x.MBox == null && x.Rating == 5).Count().Should().Equal(1);
        }

        [Theory(DisplayName = "DYNAMIC Match"), Xunit.Trait("SPARQL Query", ""),
   InlineData(@"@prefix foaf:       <http://xmlns.com/foaf/0.1/> .
                    @prefix rdf:        <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .

                    _:a  rdf:type        foaf:Person .
                    _:a  foaf:name       ""Alice"" .
                    _:a  foaf:mbox       <mailto:alice@example.com> .
                    _:a  foaf:mbox       <mailto:alice@work.example> .

                    _:b  rdf:type        foaf:Person .
                    _:b  foaf:name       ""Bob"" .
                    _:b  foaf:rating     5 .")]
        public void TestProjection2(string data)
        {
            var query = TestDataProvider.GetQuerable<Person>(data);
            var list = query.Match(s: "?x", p: "foaf:name", o: o => o.Name)
                .Optional(s: "?x", p: "foaf:mbox", o: o => o.MBox)
                .Optional(s: "?x", p: "foaf:rating", o: o => o.Rating).And(p: "foaf:rating", o: 5).End()
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(3);

            list.Where(x => x.Name == "Alice" && x.MBox == "mailto:alice@example.com").Count().Should().Equal(1);
            list.Where(x => x.Name == "Alice" && x.MBox == "mailto:alice@work.example").Count().Should().Equal(1);
            list.Where(x => x.Name == "Bob" && x.MBox == null && x.Rating == 5).Count().Should().Equal(1);
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string MBox { get; set; }
        public int Rating { get; set; }
    }




}
