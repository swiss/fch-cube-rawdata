using System.Text.RegularExpressions;
using Swiss.FCh.Cube.RawData.Contract;
using Swiss.FCh.Cube.RawData.Model;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace Swiss.FCh.Cube.RawData.Services;

internal class CubeRawDataService : ICubeRawDataService
{
    public IEnumerable<Triple> CreateTriples(
        Graph graph,
        string cubeUri,
        string observationSetUri,
        IEnumerable<ObservationDataRow> dataRows)
    {
        graph.NamespaceMap.AddNamespace("rdf",    new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
        graph.NamespaceMap.AddNamespace("schema", new Uri("http://schema.org"));
        graph.NamespaceMap.AddNamespace("cube",   new Uri("https://cube.link"));
        graph.NamespaceMap.AddNamespace("ld",     new Uri("https://ld.admin.ch"));
        graph.NamespaceMap.AddNamespace("w3",     new Uri("http://www.w3.org/"));
        graph.NamespaceMap.AddNamespace("cube",   new Uri("https://cube.link/"));

        List<string> predicatesAlreadyAddedToShape = [];

        // shape triples (constaint)
        yield return new Triple(
            graph.CreateUriNode(cubeUri),
            graph.CreateUriNode("cube:observationConstraint"),
            graph.CreateUriNode($"{cubeUri}/shape"));

        yield return new Triple(
            graph.CreateUriNode($"{cubeUri}/shape"),
            graph.CreateUriNode("rdf:type"),
            graph.CreateUriNode("w3:shacl#NodeShape"));

        yield return new Triple(
            graph.CreateUriNode($"{cubeUri}/shape"),
            graph.CreateUriNode("rdf:type"),
            graph.CreateUriNode("cube:Constraint"));

        yield return new Triple(
            graph.CreateUriNode($"{cubeUri}/shape"),
            graph.CreateUriNode("w3:ns/shacl#closed"),
            graph.CreateLiteralNode("true", UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeBoolean)));

        // definition of the cube
        yield return
            new Triple(
                graph.CreateUriNode(cubeUri),
                graph.CreateUriNode("rdf:type"),
                graph.CreateUriNode("cube:Cube"));

        yield return
            new Triple(
                graph.CreateUriNode(cubeUri),
                graph.CreateUriNode("cube:observationSet"),
                graph.CreateUriNode(observationSetUri));

        foreach (var dataRow in dataRows)
        {
            yield return
                new Triple(
                    graph.CreateUriNode(observationSetUri),
                    graph.CreateUriNode("cube:observation"),
                    graph.CreateUriNode(dataRow.KeyUri));

            yield return
                new Triple(
                    graph.CreateUriNode(dataRow.KeyUri),
                    graph.CreateUriNode("rdf:type"),
                    graph.CreateUriNode("cube:Observation"));

            yield return
                new Triple(
                    graph.CreateUriNode(dataRow.KeyUri),
                    graph.CreateUriNode("cube:observedBy"),
                    graph.CreateUriNode("ld:FCh"));

            foreach (var keyDimensionLink in dataRow.KeyDimensionLinks)
            {
                yield return
                    new Triple(
                        graph.CreateUriNode(dataRow.KeyUri),
                        graph.CreateUriNode(keyDimensionLink.PredicateUri),
                        graph.CreateUriNode(keyDimensionLink.Uri));

                if (!predicatesAlreadyAddedToShape.Contains(keyDimensionLink.PredicateUri))
                {
                    //shape (constraint) triples for links to key dimensions
                    var blankNodeId = $"blank_{CleanNodeId(keyDimensionLink.PredicateUri)}";

                    yield return new Triple(
                        graph.CreateUriNode($"{cubeUri}/shape"),
                        graph.CreateUriNode("w3:ns/shacl#property"),
                        graph.CreateBlankNode(blankNodeId));

                    yield return new Triple(
                        graph.CreateBlankNode(blankNodeId),
                        graph.CreateUriNode("w3:ns/shacl#path"),
                        graph.CreateUriNode(keyDimensionLink.PredicateUri));

                    predicatesAlreadyAddedToShape.Add(keyDimensionLink.PredicateUri);
                }
            }

            if (dataRow.ValidFrom.HasValue)
            {
                yield return
                    new Triple(
                        graph.CreateUriNode(dataRow.KeyUri),
                        graph.CreateUriNode("schema:validFrom"),
                        graph.CreateLiteralNode(dataRow.ValidFrom.Value.ToString("yyyy-MM-dd"), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeDate)));

                //shape (constraint) triples for 'valid from' date
                if (!predicatesAlreadyAddedToShape.Contains("schema:validFrom"))
                {
                    yield return new Triple(
                        graph.CreateUriNode($"{cubeUri}/shape"),
                        graph.CreateUriNode("w3:ns/shacl#property"),
                        graph.CreateBlankNode("shape_blank_validFrom"));

                    yield return new Triple(
                        graph.CreateBlankNode("shape_blank_validFrom"),
                        graph.CreateUriNode("w3:ns/shacl#path"),
                        graph.CreateUriNode("schema:validFrom"));

                    predicatesAlreadyAddedToShape.Add("schema:validFrom");
                }
            }

            if (dataRow.ValidTo.HasValue)
            {
                yield return
                    new Triple(
                        graph.CreateUriNode(dataRow.KeyUri),
                        graph.CreateUriNode("schema:validTo"),
                        graph.CreateLiteralNode(dataRow.ValidTo.Value.ToString("yyyy-MM-dd"), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeDate)));

                //shape (constraint) triples for 'valid to' date
                if (!predicatesAlreadyAddedToShape.Contains("schema:validTo"))
                {
                    yield return new Triple(
                        graph.CreateUriNode($"{cubeUri}/shape"),
                        graph.CreateUriNode("w3:ns/shacl#property"),
                        graph.CreateBlankNode("shape_blank_validTo"));

                    yield return new Triple(
                        graph.CreateBlankNode("shape_blank_validTo"),
                        graph.CreateUriNode("w3:ns/shacl#path"),
                        graph.CreateUriNode("schema:validTo"));

                    predicatesAlreadyAddedToShape.Add("schema:validTo");
                }
            }

            foreach (var dimensionValue in dataRow.Values)
            {
                yield return
                    new Triple(
                        graph.CreateUriNode(dataRow.KeyUri),
                        graph.CreateUriNode(dimensionValue.Predicate),
                        graph.CreateLiteralNode(dimensionValue.Value, dimensionValue.LanguageTag));

                var currentSchema = dimensionValue.Predicate;

                var colonIndex = currentSchema.IndexOf(':');
                var slashIndex = currentSchema.IndexOf('/');

                var index = colonIndex == -1 ? slashIndex : colonIndex;

                var blankNodeId = currentSchema.Substring(index + 1).TrimStart();

                if (!predicatesAlreadyAddedToShape.Contains(currentSchema))
                {
                    yield return new Triple(
                        graph.CreateUriNode($"{cubeUri}/shape"),
                        graph.CreateUriNode("w3:ns/shacl#property"),
                        graph.CreateBlankNode($"shape_blank_{blankNodeId}"));

                    yield return new Triple(
                        graph.CreateBlankNode($"shape_blank_{blankNodeId}"),
                        graph.CreateUriNode("w3:ns/shacl#path"),
                        graph.CreateUriNode(currentSchema));

                    predicatesAlreadyAddedToShape.Add(currentSchema);
                }
            }
        }
    }

    private static string CleanNodeId(string nodeId)
    {
        var cleaned = Regex.Replace(nodeId, "[^a-zA-Z0-9]", "_");
        return cleaned;
    }
}
