using System;
using System.Linq;
using DynamicSPARQLSpace;
using Should.Fluent;
using VDS.RDF;
using VDS.RDF.Query;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class BindFixture
    {
        [Theory(DisplayName = "Binding"),
        InlineData(@"@prefix foaf:  <http://xmlns.com/foaf/0.1/> .
            _:a  foaf:givenName   ""John"" .
            _:a  foaf:surname  ""Doe"" .")]
        public void TestBind1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data);
            var list = query.Match("?P foaf:givenName ?G")
                .And(p: "foaf:surname", o: "?S")
                .Bind("CONCAT(?G, \" \", ?S)").As("?name")
                .Select("?name")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(1);
            list.Any(x => x.name == "John Doe").Should().Be.True();
        }
    }
}
