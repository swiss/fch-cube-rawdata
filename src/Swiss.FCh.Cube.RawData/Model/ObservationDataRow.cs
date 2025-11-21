using System;
using System.Collections.Generic;

namespace Swiss.FCh.Cube.RawData.Model
{
    public class ObservationDataRow
    {
        /// <summary>
        /// URI representing a primary key or ID of the observation.
        /// </summary>
        public string KeyUri { get; set; }

        /// <summary>
        /// Key dimensions of the data row, linking it to dimensions (e.g. a person) using an URI.
        /// </summary>
        public List<KeyDimensionLink> KeyDimensionLinks { get; } = new List<KeyDimensionLink>();

        /// <summary>
        /// Used to indicate that the data row is only valid from a certain date.
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Used to indicate that the data row is only valid to a certain date.
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Holds the effective values of the observation.
        /// </summary>
        public List<DimensionValue> Values { get; } = new List<DimensionValue>();
    }
}
