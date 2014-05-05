using System.Linq;
using Should.Fluent;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class EitherFixture
    {
        [Theory(DisplayName = "Alternative Matching"), Xunit.Trait("SPARQL Query", ""),
        InlineData(@"@prefix dc10:  <http://purl.org/dc/elements/1.0/> .
            @prefix dc11:  <http://purl.org/dc/elements/1.1/> .

            _:a  dc10:title     ""SPARQL Query Language Tutorial"" .
            _:a  dc10:creator   ""Alice"" .

            _:b  dc11:title     ""SPARQL Protocol Tutorial"" .
            _:b  dc11:creator   ""Bob"" .

            _:c  dc10:title     ""SPARQL"" .
            _:c  dc11:title     ""SPARQL (updated)"" .")]
        public void TestUnion1(string data)
        {
            var query = TestDataProvider.GetQuerable<dynamic>(data);
            query = query.Prefix("dc10:", "http://purl.org/dc/elements/1.0/").Prefix("dc11:", "http://purl.org/dc/elements/1.1/");
            var list = query.Either("?book dc10:title  ?title").Or("?book dc11:title  ?title")
                .Select("?title")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(4);
            list.Any(x => x.title == "SPARQL Protocol Tutorial").Should().Be.True();
            list.Any(x => x.title == "SPARQL").Should().Be.True();
            list.Any(x => x.title == "SPARQL (updated)").Should().Be.True();
            list.Any(x => x.title == "SPARQL Query Language Tutorial").Should().Be.True();

            list = query.Either("?book dc10:title ?x").Or("?book dc11:title  ?y")
                .Select("?x ?y")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(4);
            list.Any(x => x.x == null && x.y == "SPARQL Protocol Tutorial").Should().Be.True();
            list.Any(x => x.x == null && x.y == "SPARQL (updated)").Should().Be.True();
            list.Any(x => x.x == "SPARQL" && x.y == null).Should().Be.True();
            list.Any(x => x.x == "SPARQL Query Language Tutorial" && x.y == null).Should().Be.True();

            list = query.Either("?book dc10:title ?title").And(p: "dc10:creator", o: "?author")
                .Or("?book dc11:title ?title").And(p: "dc11:creator", o: "?author").End()
                .Select("?title ?author")
                .AsEnumerable()
                .ToList();

            list.Count.Should().Equal(2);
            list.Any(x => x.author == "Alice" && x.title == "SPARQL Query Language Tutorial").Should().Be.True();
            list.Any(x => x.author == "Bob" && x.title == "SPARQL Protocol Tutorial").Should().Be.True();

        }
    }
}
