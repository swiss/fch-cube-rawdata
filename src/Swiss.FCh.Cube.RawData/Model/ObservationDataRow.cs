namespace Swiss.FCh.Cube.RawData.Model;

public class ObservationDataRow
{
    /// <summary>
    /// URI representing a primary key or ID of the observation.
    /// </summary>
    public required string KeyUri { get; init; }

    /// <summary>
    /// Key dimensions of the data row, linking it to dimensions (e.g. a person) using an URI.
    /// </summary>
    public List<KeyDimensionLink> KeyDimensionLinks { get; } = [];

    /// <summary>
    /// Used to indicate that the data row is only valid from a certain date.
    /// </summary>
    public DateTime? ValidFrom { get; init; }

    /// <summary>
    /// Used to indicate that the data row is only valid to a certain date.
    /// </summary>
    public DateTime? ValidTo { get; init; }

    /// <summary>
    /// Holds the effective values of the observation.
    /// </summary>
    public List<DimensionValue> Values { get; } = [];
}
