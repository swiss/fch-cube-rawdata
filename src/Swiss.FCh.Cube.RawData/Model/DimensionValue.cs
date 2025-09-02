namespace Swiss.FCh.Cube.RawData.Model;

public class DimensionValue
{
    public required string Predicate { get; init; }

    public required string Value { get; init; }

    public string? LanguageTag { get; init; }
}
