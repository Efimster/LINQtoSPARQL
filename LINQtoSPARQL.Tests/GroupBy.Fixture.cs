using System.Linq;
using Should.Fluent;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class GroupByFixture
    {
        [Theory(DisplayName = "Group By & Having"), Xunit.Trait("SPARQL Query", ""),
        InlineData(@"@prefix : <http://books.example/> .
            :org1 :affiliates :auth1, :auth2 .
            :auth1 :writesBook :book1, :book2 .
            :book1 :price 9 .
            :book2 :price 5 .
            :auth2 :writesBook :book3 .
            :book3 :price 7 .
            :org2 :affiliates :auth3 .
            :auth3 :writesBook :book4 .
            :book4 :price 7 .")]
        public void TestGroupBy1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data);
            var list = query.Match("?org :affiliates ?auth")
                .Match("?auth :writesBook ?book")
                .Match("?book :price ?lprice")
                .Select("SUM(?lprice) AS ?totalPrice")
                .GroupBy("?org")
                .Having("SUM(?lprice) > 10")
                .Prefix(":", "http://books.example/")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(1);
            ((int)list.First().totalPrice).Should().Equals(21);
        }
    }
}
