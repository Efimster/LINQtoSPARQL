using System.Linq;
using Should.Fluent;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class BindFixture
    {
        [Theory(DisplayName = "Binding"), Xunit.Trait("SPARQL Query", ""),
        InlineData(@"@prefix foaf:  <http://xmlns.com/foaf/0.1/> .
            _:a  foaf:givenName   ""John"" .
            _:a  foaf:surname  ""Doe"" .")]
        public void TestBind1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data);
            query = query.Match("?P foaf:givenName ?G")
                .And(p: "foaf:surname", o: "?S")
                .Bind("CONCAT(?G, \" \", ?S)").As("?name")
                .Select("?name")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/");
             var list = query.AsEnumerable().ToList();

            list.Count.Should().Equal(1);
            list.Any(x => x.name == "John Doe").Should().Be.True();
        }
    }
}
