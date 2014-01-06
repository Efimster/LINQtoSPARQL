LINQtoSPARQL is specific LINQ to SPARQ for RDF sources quering

Quering expressions:
	Match() - Triple match pattern expression (Subject Predicate Object)
	.And() - Same as Match(). Predicate and/or Subject could be ommited. In such case expression applies to previouse one.
	.Optional() - Optional match pattern expression
	.FilterBy() - Filter expression
	.Either() - Left part of Union group pattern expression
	.Or() - Right part of Union group pattern expression
	.Minus() - Minus group pattern expression
	.Exists() - Exists group pattern expression
	.NotExists() - Not Exists group pattern expression
	.Goup() - Group pattern expression (explicit). 
	.Bind() - Bind expression
	.As() - Projection of binding expression
	.Select() - Projection expression
	.GroupBy() - Group By expression
	.Having() - Having expression
	.OrderBy() - Order By expression
	.Limit() - Limit expression
	.Offset() - Offset expression
	.End() - End of group pattern expression (explicit).
	.Prefix() - Prefix expression
	.AsEnumerable() - Bridge to "LINQ to Object"


General usage:


1)Create query
	dynamic dyno = DynamicSPARQL.CreateDyno(...); //(see DynamicSPARQL project https://github.com/Efimster/DynamicSPARQL)
	ISPARQLQueryable<T>  query = new SPARQLQuery<T>(dyno);// T - could be dynamic

2) Make a query
            var list = query.Match("?org :affiliates ?auth")
                .Match("?auth :writesBook ?book")
                .Match("?book :price ?lprice")
                .Select("SUM(?lprice) AS ?totalPrice")
                .GroupBy("?org")
                .Having("SUM(?lprice) > 10")
                .Prefix(":", "http://books.example/")
                .AsEnumerable();

4) Enumerate results
		foreach(var obj in list)
			Console.WriteLine(obj.totalPrice);


Examples:

	    query.Prefix("dc", "http://purl.org/dc/elements/1.1/")
                .Prefix("ns", "http://example.org/ns#")
                .Match("?x ns:price ?price").And("dc:title ?title")
                .Select("?title ?price")
                .OrderBy("desc(?price)")
                .AsEnumerable()
                .ToList();


        query.Match(s: "?x", p: "foaf:name", o: "?name")
				.Optional(s: "?x", p: "foaf:mbox", o: "?mbox")
                .Select("?name ?mbox")
                .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
                .AsEnumerable()
                .ToList();

        query.Match("?x foaf:name  ?name")
		       .Optional("?x foaf:mbox ?mbox")
			   .Optional("?x foaf:homepage ?hpage")
               .Select("?name ?mbox ?hpage")
               .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
               .AsEnumerable()
               .ToList();

	    query.Match("?x dc:title ?title")
				.Optional("?x ns:price ?price")
					.FilterBy("?price < 30")
				.End()
                .Select("?title ?price")
                .Prefix("dc:", "http://purl.org/dc/elements/1.1/")
                .Prefix("ns:", "http://example.org/ns#")
                .AsEnumerable()
                .ToList();
            
		query.Match("?x ns:price ?price").And(p: "dc:title", o: "?title")
                .Select("?title ?price")
                .OrderBy("desc(?price)")
                .Limit(1)
                .Offset(1)
                .Prefix("dc", "http://purl.org/dc/elements/1.1/")
                .Prefix("ns", "http://example.org/ns#")
                .AsEnumerable()
                .ToList();

        query.Either("?book dc10:title  ?title").Or("?book dc11:title  ?title")
                .Select("?title")
                .AsEnumerable()
                .ToList();

		query.Match("?P foaf:givenName ?G")
                .And(p: "foaf:surname", o: "?S")
                .Bind("CONCAT(?G, \" \", ?S)").As("?name")
                .Select("?name")
                .Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
                .AsEnumerable()
                .ToList();


see other examles within unit tests.

Typed projection:

ISPARQLQueryable<Person> query;
Person person = query.Match(s: "?x", p: "foaf:name", o: o => o.Name)
			.Optional(s: "?x", p: "foaf:mbox", o: o => o.MBox)
			.Optional(s: "?x", p: "foaf:rating", o: o => o.Rating).And(p: "foaf:rating", o: 5).End()
			.Prefix("foaf:", "http://xmlns.com/foaf/0.1/")
			//Select() - select expression could be ommited
			.AsEnumerable()
			.First();

SPARQL Update:

1)Create query
	dynamic dyno = DynamicSPARQL.CreateDyno(...); //(see DynamicSPARQL project https://github.com/Efimster/DynamicSPARQL)
	ISPARQLQueryable<T>  query = new SPARQLQuery<T>(dyno);// T - could be dynamic
2) Make a query
    //DELETE DATA
	var updRes = query.Delete()
						.Match(s: "<http://example/book2>", p: "dc:title", o: @"""David Copperfield""")
						.And(p: "dc:creator", o: @"""Edmund Wells""")
                    .Prefix("dc", "http://purl.org/dc/elements/1.1/")
                    .ExecuteUpdate();
	//INSERT DATA
	query.Prefix("dc", "http://purl.org/dc/elements/1.1/")
                .Insert()
                    .Match(s: "<http://example/book1>", p: "dc:title", o: @"""David Copperfield""")
                    .And(p: "dc:creator", o: @"""Edmund Wells""")
                .ExecuteUpdate();
	//DELETE/INSERT
	query.Match("?person foaf:givenName Bill")
                .Delete().Match("?person foaf:givenName Bill")
                .Insert().Match("?person foaf:givenName William")
                .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
                .ExecuteUpdate();
	//DELETE(informative)
	query.Match("?book dc:date ?date")
                .FilterBy(@"?date > ""1970-01-01T00:00:00-02:00""^^xsd:dateTime")
                .Match("?book ?p ?v")
                .Delete().Match("?book ?p ?v")
                .Prefix("dc", "http://purl.org/dc/elements/1.1/")
                .Prefix("xsd", "http://www.w3.org/2001/XMLSchema#")
                .ExecuteUpdate();
	//INSERT(informative)
	 query.Match("?person  foaf:name  ?name")
                .Optional("?person  foaf:mbox  ?email")
                .Insert()
                    .Match("?person  foaf:name2  ?name")
                    .Match("?person  foaf:mbox2  ?email")
                .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
                .ExecuteUpdate();
	//DELETE WHERE
	 query.Match("?person foaf:givenName Fred").And("?property ?value")
                .Delete()
                .Prefix("foaf", "http://xmlns.com/foaf/0.1/")
                .ExecuteUpdate();
