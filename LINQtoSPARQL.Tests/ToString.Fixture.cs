using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DynamicSPARQLSpace;
using Should.Fluent;
using VDS.RDF;
using VDS.RDF.Query;
using Xunit;
using Xunit.Extensions;

namespace LINQtoSPARQLSpace.Tests
{
    public class ToStringFixture
    {
        [Fact(DisplayName = "Prefixes"), Xunit.Trait("ToString()", "")]
        public void TestMerge1()
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

    }
}
