using System.Collections.Generic;
using Swiss.FCh.Cube.RawData.Model;
using VDS.RDF;

namespace Swiss.FCh.Cube.RawData.Contract
{
    /// <summary>
    /// This service is used to create a "raw data" (data rows of cubes) according to the <a href="https://cube.link">https://cube.link</a> schema.
    /// </summary>
    public interface ICubeRawDataService
    {
        /// <summary>
        /// Writes two dimensional raw data to an RDF cube according to the <a href="https://cube.link">https://cube.link</a> schema.
        /// </summary>
        /// <param name="graph">RDF graph where the triples will be added.</param>
        /// <param name="cubeUri">Uri of the cube/></param>
        /// <param name="dataRows">Row data to write to the cube <see cref="DimensionValue"/></param>
        /// <returns>The triples that can be added to the graph</returns>
        IEnumerable<Triple> CreateTriples(
            Graph graph,
            string cubeUri,
            IEnumerable<ObservationDataRow> dataRows);
    }
}
