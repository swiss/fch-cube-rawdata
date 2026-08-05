namespace Swiss.FCh.Cube.RawData.Model
{
    /// <summary>
    /// This class defines the available shacl shape metadata for the cube's properties.
    /// </summary>
    public class ShapePropertyMetadata
    {
        /// <summary>
        /// Specyfies the kind of the node (e.g. "w3:ns/shacl#IRI" or "w3:ns/shacl#Literal").
        /// </summary>
        public string NodeKind { get; set; }

        /// <summary>
        /// Specyfies the data type of the property.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Specyfies the german name of the property.
        /// </summary>
        public string NameDe { get; set; }

        /// <summary>
        /// Specyfies the french name of the property.
        /// </summary>
        public string NameFr { get; set; }

        /// <summary>
        /// Specyfies the italian name of the property.
        /// </summary>
        public string NameIt { get; set; }

        /// <summary>
        /// Specyfies the english name of the property.
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// Specyfies the scale type (e.g. "qudt:IntervalScale").
        /// </summary>
        public string ScaleType { get; set; }

        /// <summary>
        /// Specyfies the minimum value that is expected for the property.
        /// </summary>
        public int? MinCount { get; set; }

        /// <summary>
        /// Specyfies the maximum value that is expected for the property.
        /// </summary>
        public int? MaxCount { get; set; }
    }
}
