namespace Swiss.FCh.Cube.RawData.Model
{
    /// <summary>
    /// Use this class, if your raw data row needs to reference a dimension value (created by fch-cube-dimension).
    /// </summary>
    public class KeyDimensionLink
    {
        /// <summary>
        /// The RDF predicate linking to the dimension value.
        /// </summary>
        public string Predicate { get; set; }

        /// <summary>
        /// The uri of the referenced dimension value.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Sets Shacl shape metadata for the cube's properties.
        /// </summary>
        public ShapePropertyMetadata ShapePropertyMetadata { get; set; }
    }
}
