using System.Linq;
using Should.Fluent;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class OrderByFixture
    {
        [Theory(DisplayName = "Order by "), Xunit.Trait("SPARQL Query", ""),
        InlineData(@"@prefix dc:   <http://purl.org/dc/elements/1.1/> .
                @prefix :     <http://example.org/book/> .
                @prefix ns:   <http://example.org/ns#> .

                :book1  dc:title  ""SPARQL Tutorial"" .
                :book1  ns:price  42 .
                :book2  dc:title  ""The Semantic Web"" .
                :book2  ns:price  23 .")]
        public void TestOrderBy1(string data)
        {
            var query =  TestDataProvider.GetQuerable<dynamic>(data);

            var list = query.Prefix("dc:", "http://purl.org/dc/elements/1.1/")
                .Prefix("ns:", "http://example.org/ns#")
                .Match("?x ns:price ?price").Match("?x dc:title ?title")
                .Select("?title ?price")
                .OrderBy("desc(?price)")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(2);
            ((int)list.First().price).Should().Equals(42);

        }
    }
}
