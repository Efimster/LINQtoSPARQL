LINQtoSPARQL is specific LINQ to SPARQ for RDF sources quering


General usage:

var list = query.Match(s: "?x", p: "dc:title", o: "?title").FilterBy("regex(?title, \"^SPARQL\")")
                .Select("?title")
                .Prefix("dc:", "http://purl.org/dc/elements/1.1/")
                .AsEnumerable()
                .ToList();