using Should.Fluent;
using Xunit;

namespace LINQtoSPARQLSpace.Tests
{
    public class MergeFixture
    {
        [Fact(DisplayName = "Two WHEREs"), Xunit.Trait("Merge queries", "")]
        public void TestMerge1()
        {
            var source = TestDataProvider.GetQuerable<dynamic>("");
            var query = source.Match("?x ns:price ?price").And("dc:title ?title");
            var query2 = source.Match("?y ns:price ?price").And("dc:title ?title");

            var resStr = query.Merge(query2).ToString();

            resStr.Should().Contain(
@"WHERE {
?x ns:price ?price; dc:title ?title .
?y ns:price ?price; dc:title ?title .
}"
            );

        }
        [Fact(DisplayName = "Two DELETEs"), Xunit.Trait("Merge queries", "")]
        public void TestMerge2()
        {
            var source = TestDataProvider.GetQuerable<dynamic>("", autoquotation:false);
            var query = source.Delete().Match("?d1 ?d2 ?d3").And("?d4 ?d5");
            var query2 = source.Delete().Match("?d6 ?d7 ?d8").And("?d9 ?d10");

            var resStr = query.Merge(query2).ToString();

            resStr.Should().Contain(
@"DELETE {
?d1 ?d2 ?d3; ?d4 ?d5 .
?d6 ?d7 ?d8; ?d9 ?d10 .
}"
            );

            resStr = query2.Merge(query).ToString();

            resStr.Should().Contain(
@"DELETE {
?d6 ?d7 ?d8; ?d9 ?d10 .
?d1 ?d2 ?d3; ?d4 ?d5 .
}"
            );

        }

        [Fact(DisplayName = "Two INSERTs"), Xunit.Trait("Merge queries", "")]
        public void TestMerge3()
        {
            var source = TestDataProvider.GetQuerable<dynamic>("", autoquotation:false);
            var query = source.Insert().Match("?i1 ?i2 ?i3").And("?i4 ?i5");
            var query2 = source.Insert().Match("?i6 ?i7 ?i8").And("?i9 ?i10");

            var resStr = query.Merge(query2).ToString();

            resStr.Should().Contain(
@"INSERT {
?i1 ?i2 ?i3; ?i4 ?i5 .
?i6 ?i7 ?i8; ?i9 ?i10 .
}"
            );

            resStr = query2.Merge(query).ToString();

            resStr.Should().Contain(
@"INSERT {
?i6 ?i7 ?i8; ?i9 ?i10 .
?i1 ?i2 ?i3; ?i4 ?i5 .
}"
            );

        }

        [Fact(DisplayName = "DELETE and WHERE"), Xunit.Trait("Merge queries", "")]
        public void TestMerge4()
        {
            var source = TestDataProvider.GetQuerable<dynamic>("");
            var query = source.Match("?w1 ?w2 ?w3").And("?w4 ?w5");
            var query2 = source.Delete().Match("?d1 ?d2 ?d3").And("?d4 ?d5");

            var resStr = query.Merge(query2).ToString();

            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3; ?w4 ?w5 .
}"
            ).Should().Contain(
@"DELETE {
?d1 ?d2 ?d3; ?d4 ?d5 .
}"
            );

            resStr = query2.Merge(query).ToString();
            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3; ?w4 ?w5 .
}"
            ).Should().Contain(
@"DELETE {
?d1 ?d2 ?d3; ?d4 ?d5 .
}"
            );

            var query3 = source.Delete();
            
            resStr = query.Merge(query3).ToString();
            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3; ?w4 ?w5 .
}"
            ).Should().Not.Contain("DELETE");

            resStr = query3.Merge(query).ToString();
            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3; ?w4 ?w5 .
}"
            ).Should().Not.Contain("DELETE");

        }

        [Fact(DisplayName = "INSERT and WHERE"), Xunit.Trait("Merge queries", "")]
        public void TestMerge5()
        {
            var source = TestDataProvider.GetQuerable<dynamic>("",autoquotation:false);
            var query = source.Match("?w1 ?w2 ?w3").And("?w4 ?w5");
            var query2 = source.Insert().Match("?i2 ?i2 ?i3").And("?i4 ?i5");

            var resStr = query.Merge(query2).ToString();

            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3; ?w4 ?w5 .
}"
            ).Should().Contain(
@"INSERT {
?i2 ?i2 ?i3; ?i4 ?i5 .
}"
            );

            resStr = query2.Merge(query).ToString();

            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3; ?w4 ?w5 .
}"
            ).Should().Contain(
@"INSERT {
?i2 ?i2 ?i3; ?i4 ?i5 .
}"
            );

        }

        [Fact(DisplayName = "WHERE DELETE INSERT and WHERE DELETE INSERT"), Xunit.Trait("Merge queries", "")]
        public void TestMerge6()
        {
            var source = TestDataProvider.GetQuerable<dynamic>("");
            var query = source.Match("?w1 ?w2 ?w3").Insert().Match("?i1 ?i2 ?i3").Delete().Match("?d1 ?d2 ?d3").And("?d4 ?d5");
            var query2 = source.Match("?w4 ?w5 ?w6").And("?w7 ?w8").Delete().Match("?d6 ?d7 ?d8").Insert().Match("?i4 ?i5 ?i6");
            

            var resStr = query.Merge(query2).ToString();

            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3 .
?w4 ?w5 ?w6; ?w7 ?w8 .
}"
            ).Should().Contain(
@"DELETE {
?d1 ?d2 ?d3; ?d4 ?d5 .
?d6 ?d7 ?d8 .
}"
            ).Should().Contain(
@"INSERT {
?i1 ?i2 ?i3 .
?i4 ?i5 ?i6 .
}"
            );

            resStr = query2.Merge(query).ToString();

            resStr.Should().Contain(
@"WHERE {
?w4 ?w5 ?w6; ?w7 ?w8 .
?w1 ?w2 ?w3 .
}"
            ).Should().Contain(
@"DELETE {
?d6 ?d7 ?d8 .
?d1 ?d2 ?d3; ?d4 ?d5 .
}"
            ).Should().Contain(
@"INSERT {
?i4 ?i5 ?i6 .
?i1 ?i2 ?i3 .
}"
            );

        }

        [Fact(DisplayName = "Emptyies"), Xunit.Trait("Merge queries", "")]
        public void TestMerge7()
        {
            var source = TestDataProvider.GetQuerable<dynamic>("",autoquotation:false);
            var query = source;
            var query2 = source;

            var resStr = query.Merge(query2).ToString();

            resStr.Should().Be.Empty();

            var query3 = source.Match("?w1 ?w2 ?w3");

            resStr = query.Merge(query3).ToString();
            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3 .
}"
            );

            resStr = query3.Merge(query).ToString();
            resStr.Should().Contain(
@"WHERE {
?w1 ?w2 ?w3 .
}"
            );

        }


    }
}
