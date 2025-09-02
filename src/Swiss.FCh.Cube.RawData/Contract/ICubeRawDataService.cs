using System.Data;
using Swiss.FCh.Cube.RawData.Model;
using VDS.RDF;

namespace Swiss.FCh.Cube.RawData.Contract;

public interface ICubeRawDataService
{
    /// <summary>
    /// Writes two dimensional raw data to an RDF cube (see: https://cube.link).
    /// </summary>
    /// <param name="graph">RDF graph where the triples will be added.</param>
    /// <param name="cubeUri">Uri of the cube/></param>
    /// <param name="observationSetUri">Uri of the observation set</param>
    /// <param name="dataRows">Row data to write to the cube <see cref="Bk.Cube.RawData.Model.DimensionValue"/></param>
    /// <returns>The triples that can be added to the graph</returns>
    IEnumerable<Triple> CreateTriples(
        Graph graph,
        string cubeUri,
        string observationSetUri,
        IEnumerable<ObservationDataRow> dataRows);
}
