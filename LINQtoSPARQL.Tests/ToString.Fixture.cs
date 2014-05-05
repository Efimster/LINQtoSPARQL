using DynamicSPARQLSpace;
using Should.Fluent;
using Xunit;

namespace LINQtoSPARQLSpace.Tests
{
    public class ToStringFixture
    {
        [Fact(DisplayName = "Prefixes"), Xunit.Trait("ToString()", "")]
        public void TestToString1()
        {
            var prefixes = new[]{
                SPARQL.Prefix("p1","http://test.com/p1"),
                SPARQL.Prefix("p2","http://test.com/p2"),
            };
            
            
            
            var source = TestDataProvider.GetQuerable<dynamic>("", prefixes: prefixes);

            var query = source.Prefix("p3","http://test.com/p3")
                        .Prefix("p4","http://test.com/p4")
                        .Prefix("p5","http://test.com/p5");

            var resStr = query.ToString();

            resStr.Should().StartWith(
@"PREFIX p1: <http://test.com/p1>
PREFIX p2: <http://test.com/p2>
PREFIX p3: <http://test.com/p3>
PREFIX p4: <http://test.com/p4>
PREFIX p5: <http://test.com/p5>"
            );

        }

        [Fact(DisplayName = "Skip Triples With Empty Object"), Xunit.Trait("ToString()", "")]
        public void TestToString2()
        {

            var source = TestDataProvider.GetQuerable<dynamic>("", skipTriplesWithEmptyObject:true);

            var query = source
                        .Match("book:c2091b57-8d96-45da-ad81-157f9630cd5f", "prop:name", "\"\"")
                            .And("prop:concern", "\"\"");

            var resStr = query.ToString();

            resStr.Should().Not.Contain("book");

        }

    }
}
