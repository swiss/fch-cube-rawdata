# Changelog

## Version 2.0.4

- Extend DimensionValue with ShapePropertyMetadata

## Version 2.0.3

- Resolve qudt namespace

## Version 2.0.2

- Extend KeyDimensionLink with ShapePropertyMetadata

## Version 2.0.1

- Introduce DataType to DimensionValue

## Version 2.0.0

BREAKING CHANGE: multiple renamings

- IServiceCollection.AddCubeRawData -> IServiceCollection.AddRawDataService
- ICubeRawDataService -> IRawDataService
- DimensionValue.Value -> DimensionValue.Object
- KeyDimensionLink.PredicateUri -> KeyDimensionLink.PredicateUri

IRawDataService.CreateTriples: observationSetUri param has been removed.

Static code analysis (Roslyn Rules) enabled.

## Version 1.0.1

fix: add proper metadata for values properties #BKDO-1565

## Version 1.0.0

Initial publication on GitHub
