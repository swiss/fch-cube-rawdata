namespace Swiss.FCh.Cube.RawData.Model
{
    /// <summary>
    /// This class is used to add a literal or numeric value to your raw data row.
    /// </summary>
    public class DimensionValue
    {
        /// <summary>
        /// The RDF predicate of this value.
        /// </summary>
        public string Predicate { get; set; }

        /// <summary>
        /// The RDF object (value).
        /// </summary>
        public string Object { get; set; }

        /// <summary>
        /// If it is a literal value, this property allows to set the corresponding language tag.
        /// </summary>
        public string LanguageTag { get; set; }

        /// <summary>
        /// With this property, the RDF data type can be specyfied.
        /// </summary>
        public string DataTypeUri { get; set; }

        /// <summary>
        /// Sets Shacl shape metadata for the cube's properties.
        /// </summary>
        public ShapePropertyMetadata ShapePropertyMetadata { get; set; }
    }
}
