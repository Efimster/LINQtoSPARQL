using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions;
using Should.Fluent;

namespace LINQtoSPARQLSpace.Tests
{
    public class UpdateFixture
    {
        [Theory(DisplayName = "DELETE DATA"),
        Xunit.Trait("SPARQL Update", ""),
        InlineData(@"@prefix dc: <http://purl.org/dc/elements/1.1/> .
            @prefix ns: <http://example.org/ns#> .

            <http://example/book2> ns:price 42 .
            <http://example/book2> dc:title ""David Copperfield"" .
            <http://example/book2> dc:creator ""Edmund Wells"" .")]
        public void TestDeleteData1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: false);

            var updRes = query.Delete()
                    .Match(s: "<http://example/book2>", p: "dc:title", o: @"""David Copperfield""")
                    .And(p: "dc:creator", o: @"""Edmund Wells""")
                    .Prefix("dc", "http://purl.org/dc/elements/1.1/")
                    .ExecuteUpdate();

            updRes.Should().Equal(0);
            query.LastQueryPrint.Should().Contain("DELETE DATA");

            IEnumerable<dynamic> res = query.Match("?s ?p ?o").AsEnumerable();

            var list = res.ToList();
            list.Count.Should().Equal(1);
            list.Any(x => x.p == "price" && x.o == 42).Should().Be.True();
        }

        [Theory(DisplayName = "INSERT DATA"),
         Xunit.Trait("SPARQL Update", ""),
         InlineData(@"@prefix dc: <http://purl.org/dc/elements/1.1/> .
                @prefix ns: <http://example.org/ns#> .

                <http://example/book1> ns:price 42 .")]
        public void TestInsertData(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: false);

            query.Prefix("dc", "http://purl.org/dc/elements/1.1/")
                .Insert()
                    .Match(s: "<http://example/book1>", p: "dc:title", o: @"""David Copperfield""")
                    .And(p: "dc:creator", o: @"""Edmund Wells""")
                .ExecuteUpdate();

            IEnumerable<dynamic> res = query.Match("?s ?p ?o").AsEnumerable();

            query.LastQueryPrint.Should().Contain("INSERT DATA");
            var list = res.ToList();
            list.Count.Should().Equal(3);
            list.Where(x => x.p == "price" && x.o == 42).Count().Should().Equal(1);
            list.Where(x => x.p == "title" && x.o == "David Copperfield").Count().Should().Equal(1);
            list.Where(x => x.p == "creator" && x.o == "Edmund Wells").Count().Should().Equal(1);
        }

        [Theory(DisplayName = "DELETE/INSERT"),
         Xunit.Trait("SPARQL Update", ""),
         InlineData(@"@prefix foaf:  <http://xmlns.com/foaf/0.1/> .
            <http://example/president25> foaf:givenName ""Bill"" .
            <http://example/president25> foaf:familyName ""McKinley"" .
            <http://example/president27> foaf:givenName ""Bill"" .
            <http://example/president27> foaf:familyName ""Taft"" .
            <http://example/president42> foaf:givenName ""Bill"" .
            <http://example/president42> foaf:familyName ""Clinton"" .")]
        public void TestDeleteInsert(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: true);

            query.Match("?person foaf:givenName Bill")
                .Delete().Match("?person foaf:givenName Bill")
                .Insert().Match("?person foaf:givenName William")
                .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
                .ExecuteUpdate();

            IEnumerable<dynamic> res = query.Match("?s ?p ?o").AsEnumerable();

            query.LastQueryPrint.Should().Contain("DELETE").Should().Contain("INSERT").Should().Contain("WHERE");

            var list = res.ToList();
            list.Count.Should().Equal(6);
            list.Where(x => x.p == "givenName" && x.o == "William").Count().Should().Equal(3);
            list.Where(x => x.p == "givenName" && x.o == "Bill").Count().Should().Equal(0);
        }

        [Theory(DisplayName = "DELETE(Informative)"),
         Xunit.Trait("SPARQL Update", ""),
         InlineData(@"@prefix dc: <http://purl.org/dc/elements/1.1/> .
            @prefix xsd: <http://www.w3.org/2001/XMLSchema#> .
            @prefix ns: <http://example.org/ns#> .

            <http://example/book1> dc:title ""Principles of Compiler Design"" .
            <http://example/book1> dc:date ""1977-01-01T00:00:00-02:00""^^xsd:dateTime .

            <http://example/book2> ns:price 42 .
            <http://example/book2> dc:title ""David Copperfield"" .
            <http://example/book2> dc:creator ""Edmund Wells"" .
            <http://example/book2> dc:date ""1948-01-01T00:00:00-02:00""^^xsd:dateTime .

            <http://example/book3> dc:title ""SPARQL 1.1 Tutorial"" .")]
        public void TestDeleteInformative(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: false);

            query.Match("?book dc:date ?date")
                .FilterBy(@"?date > ""1970-01-01T00:00:00-02:00""^^xsd:dateTime")
                .Match("?book ?p ?v")
                .Delete().Match("?book ?p ?v")
                .Prefix("dc", "http://purl.org/dc/elements/1.1/")
                .Prefix("xsd", "http://www.w3.org/2001/XMLSchema#")
                .ExecuteUpdate();

            IEnumerable<dynamic> res = query.Match("?s ?p ?o").AsEnumerable();

            query.LastQueryPrint.Should().Contain("DELETE").Should().Contain("WHERE");
            var list = res.ToList();
            list.Count.Should().Equal(5);
            list.Where(x => x.s == "book2").Count().Should().Equal(4);
            list.Where(x => x.s == "book3").Count().Should().Equal(1);
            list.Where(x => x.s == "book1").Count().Should().Equal(0);

        }

        [Theory(DisplayName = "INSERT(Informative)"),
         Xunit.Trait("SPARQL Update", ""),
         InlineData(@"@prefix foaf: <http://xmlns.com/foaf/0.1/> .
            @prefix rdf:  <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .

            _:a  rdf:type        foaf:Person .
            _:a  foaf:name       ""Alice"" .
            _:a  foaf:mbox       <mailto:alice@example.com> .

            _:b  rdf:type        foaf:Person .
            _:b  foaf:name       ""Bob"" .")]
        public void TestInsertInformative(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: false);

            query.Match("?person  foaf:name  ?name")
                .Optional("?person  foaf:mbox  ?email")
                .Insert()
                    .Match("?person  foaf:name2  ?name")
                    .Match("?person  foaf:mbox2  ?email")
                .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
                .ExecuteUpdate();

            IEnumerable<dynamic> res = query.Match("?s ?p ?o").AsEnumerable();

            query.LastQueryPrint.Should().Contain("INSERT").Should().Contain("WHERE");
            var list = res.ToList();
            list.Count.Should().Equal(8);
            list.Where(x => x.p == "mbox2").Count().Should().Equal(1);
            list.Where(x => x.p == "name2").Count().Should().Equal(2);


        }

        [Theory(DisplayName = "DELETE WHERE"),
         Xunit.Trait("SPARQL Update", ""),
         InlineData(@"@prefix foaf:  <http://xmlns.com/foaf/0.1/> .

            <http://example/william> a foaf:Person .
            <http://example/william> foaf:givenName ""William"" .
            <http://example/william> foaf:mbox  <mailto:bill@example> .

            <http://example/fred> a foaf:Person .
            <http://example/fred> foaf:givenName ""Fred"" .
            <http://example/fred> foaf:mbox  <mailto:fred@example> .")]
        public void TestDeleteWhere(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data, autoquotation: true);

            query.Match("?person foaf:givenName Fred").And("?property ?value")
                .Delete()
                .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
                .ExecuteUpdate();

            query.LastQueryPrint.Should().Contain("DELETE WHERE");

            IEnumerable<dynamic> res = query.Match("?s ?p ?o").AsEnumerable();

            var list = res.ToList();
            list.Count.Should().Equal(3);
            list.Where(x => x.o == "William" && x.p == "givenName").Count().Should().Equal(1);
            list.Where(x => x.o == "mailto:bill@example" && x.p == "mbox").Count().Should().Equal(1);

        }
    }
}
