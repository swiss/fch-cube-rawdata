using Swiss.FCh.Cube.RawData.Model;
using Swiss.FCh.Cube.RawData.Services;
using VDS.RDF;
using VDS.RDF.Nodes;

namespace Swiss.FCh.Cube.RawData.Tests.Services;

[TestFixture]
internal sealed class RawDataServiceTests
{
    private readonly RawDataService _cubeRawDataService = new();

    [Test]
    public void CreateTriples_WithValidInput_ReturnsTriplesCorrectly()
    {
        using Graph graph = new();

        const string cubeUri = "example:cube";

        graph.NamespaceMap.AddNamespace("example", new Uri("http://example.com/"));

        List<ObservationDataRow> dataRows =
            [
                new()
                {
                    KeyUri = "example:key/1",
                    ValidFrom = new DateTime(2020, 1, 1),
                    ValidTo = new DateTime(2020, 2, 2)
                },
                new()
                {
                    KeyUri = "example:key/2",
                    ValidFrom = new DateTime(2021, 1, 1),
                    ValidTo = new DateTime(2021, 2, 2)
                }
            ];

        dataRows[0].KeyDimensionLinks.Add(
            new KeyDimensionLink { Predicate = "example:hasProperty", Uri = "example:someValue"});

        dataRows[0].Values.Add(new DimensionValue { Predicate = "example:hasSomeOtherProperty", Object = "a value"});

        dataRows[0].Values.Add(new DimensionValue { Predicate = "example:hasSomeOtherLangProperty", Object = "this is text", LanguageTag = "de"});

        dataRows[0].Values.Add(new DimensionValue { Predicate = "example:decimalValue", Object = "50.50", LanguageTag = null, DataType = "http://www.w3.org/2001/XMLSchema#decimal" });

        dataRows[0].Values.Add(new DimensionValue { Predicate = "example:numberValue", Object = "1", DataType = "http://www.w3.org/2001/XMLSchema#integer" });

        dataRows[1].KeyDimensionLinks.Add(
            new KeyDimensionLink { Predicate = "example:hasProperty", Uri = "example:someOtherValue"});

        var result = _cubeRawDataService.CreateTriples(graph, cubeUri, dataRows).ToList();

        Assert.That(result, Is.Not.Null);

        //validate shape / constraint
        ValidateTriple(result, "http://example.com/cube", "https://cube.link/observationConstraint", "http://example.com/cube/shape", "Link to shape must be present");
        ValidateTriple(result, "http://example.com/cube/shape", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", "http://www.w3.org/shacl#NodeShape", "shape must be set as a shape");
        ValidateTriple(result, "http://example.com/cube/shape", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", "https://cube.link/Constraint", "shape must be a constraint");
        ValidateTriple(result, "http://example.com/cube/shape", "http://www.w3.org/ns/shacl#closed", "true", "shape must have shacl poperty 'closed'");

        //validate definition of the cube
        ValidateTriple(result, "http://example.com/cube", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", "https://cube.link/Cube", "Cube type must be set");
        ValidateTriple(result, "http://example.com/cube", "https://cube.link/observationSet", "http://example.com/cube/observationSet", "Cube must have an observation set");

        //validate data rows 0
        ValidateTriple(result, "http://example.com/cube/observationSet", "https://cube.link/observation", "http://example.com/key/1", "Observation set must have an observation");
        ValidateTriple(result, "http://example.com/key/1", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", "https://cube.link/Observation", "Type of observation must be set");
        ValidateTriple(result, "http://example.com/key/1", "https://cube.link/observedBy", "https://ld.admin.ch/FCh", "Observation must have 'observed by' property");
        ValidateTriple(result, "http://example.com/key/1", "http://example.com/hasProperty", "http://example.com/someValue", "Observation must have linkt to a key dimension");
        ValidateTriple(result, "http://example.com/key/1", "http://schema.org/validFrom", "2020-01-01", "valid from of key/1 must be set");
        ValidateTriple(result, "http://example.com/key/1", "http://schema.org/validTo", "2020-02-02", "valid to of key/1 must be set");

        //validate data row 1
        ValidateTriple(result, "http://example.com/cube/observationSet", "https://cube.link/observation", "http://example.com/key/2", "Observation set must have an observation");
        ValidateTriple(result, "http://example.com/key/2", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", "https://cube.link/Observation", "Type of observation must be set");
        ValidateTriple(result, "http://example.com/key/2", "https://cube.link/observedBy", "https://ld.admin.ch/FCh", "Observation must have 'observed by' property");
        ValidateTriple(result, "http://example.com/key/2", "http://example.com/hasProperty", "http://example.com/someOtherValue", "Observation must have linkt to a key dimension");
        ValidateTriple(result, "http://example.com/key/2", "http://schema.org/validFrom", "2021-01-01", "valid from of key/2 must be set");
        ValidateTriple(result, "http://example.com/key/2", "http://schema.org/validTo", "2021-02-02", "valid to of key/2 must be set");

        //shacl path for 'hasProperty' (must be written only once)
        ValidateTriple(result, "http://example.com/cube/shape", "http://www.w3.org/ns/shacl#property", "_:blank_example_hasProperty", "Shape must contain blank not referencing 'hasProperty'");
        ValidateTriple(result, "_:blank_example_hasProperty", "http://www.w3.org/ns/shacl#path", "http://example.com/hasProperty", "blank node for 'hasProperty' must have a path attached");

        //shacl path for 'validFrom'
        ValidateTriple(result, "http://example.com/cube/shape", "http://www.w3.org/ns/shacl#property", "_:shape_blank_validFrom", "must have blank node for 'validFrom' path");
        ValidateTriple(result, "_:shape_blank_validFrom", "http://www.w3.org/ns/shacl#path", "http://schema.org/validFrom", "must have shacl path for 'validFrom'");

        //shacl path for 'validTo'
        ValidateTriple(result, "http://example.com/cube/shape", "http://www.w3.org/ns/shacl#property", "_:shape_blank_validTo", "must have blank node for 'validFrom' path");
        ValidateTriple(result, "_:shape_blank_validTo", "http://www.w3.org/ns/shacl#path", "http://schema.org/validTo", "must have shacl path for 'validFrom'");

        //Validate values
        ValidateTriple(result, "http://example.com/key/1", "http://example.com/hasSomeOtherProperty", "a value", "'normal' values must be added as triples");
        ValidateTriple(result, "_:shape_blank_hasSomeOtherProperty", "http://www.w3.org/ns/shacl#path", "http://example.com/hasSomeOtherProperty", "must have shacl path for 'a value'");

        ValidateTriple(result, "http://example.com/key/1", "http://example.com/hasSomeOtherLangProperty", "this is text", "'normal' values with language tags must be added as triples", langTag: "de");
        ValidateTriple(result, "_:shape_blank_hasSomeOtherLangProperty", "http://www.w3.org/ns/shacl#path", "http://example.com/hasSomeOtherLangProperty", "must have shacl path for 'this is text'");

        ValidateDataTypeTriple(result, "http://example.com/key/1", "http://example.com/decimalValue", "50.50", "'numeric' values must be added as triples with uri", dataType: "http://www.w3.org/2001/XMLSchema#decimal");
        ValidateTriple(result, "_:shape_blank_decimalValue", "http://www.w3.org/ns/shacl#path", "http://example.com/decimalValue", "must have shacl path for '50.50'");

        ValidateDataTypeTriple(result, "http://example.com/key/1", "http://example.com/numberValue", "1", "'numeric' values must be added as triples with uri", dataType: "http://www.w3.org/2001/XMLSchema#integer");
        ValidateTriple(result, "_:shape_blank_numberValue", "http://www.w3.org/ns/shacl#path", "http://example.com/numberValue", "must have shacl path for '1'");
    }

    private static void ValidateTriple(IEnumerable<Triple> triples, object s, object p, object o, string failMessage, string? langTag = null)
    {
        Assert.That(
            triples.Any
            (t =>
                {
                    var subjectMatches = MatchNode(t.Subject, s);
                    var predicateMatches = MatchNode(t.Predicate, p);
                    var objectMatches = MatchNode(t.Object, o, langTag);

                    return subjectMatches && predicateMatches && objectMatches;
                }
            ),
            failMessage);
    }

    private static void ValidateDataTypeTriple(IEnumerable<Triple> triples, object s, object p, object o, string failMessage, string dataType)
    {
        Assert.That(
            triples.Any
            (t =>
            {
                var subjectMatches = MatchNode(t.Subject, s);
                var predicateMatches = MatchNode(t.Predicate, p);
                var objectMatches = MatchNode(t.Object, o, dataType);

                return subjectMatches && predicateMatches && objectMatches;
            }
            ),
            failMessage);
    }

    private static bool MatchNode(INode n, object expected, string? langTag = null)
    {
        if (n is LiteralNode literal && !string.IsNullOrEmpty(literal.Language))
        {
            return literal.Value.Equals(expected) && literal.Language == langTag;
        }

        if (n.NodeType == NodeType.Blank)
        {
            return n.ToString().Equals(expected);
        }

        return n.AsValuedNode().AsString().Equals(expected);
    }
}
