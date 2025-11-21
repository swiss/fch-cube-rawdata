# Changelog

## Version 1.0.2
BREAKING CHANGE: multiple renamings
- IServiceCollection.AddCubeRawData -> IServiceCollection.AddRawDataService
- ICubeRawDataService -> IRawDataService
- DimensionValue.Value -> DimensionValue.Object
- KeyDimensionLink.PredicateUri -> KeyDimensionLink.PredicateUri

IRawDataService.CreateTriples: observationSetUri param has been removed

## Version 1.0.1
fix: add proper metadata for values properties #BKDO-1565

## Version 1.0.0
Initial publication on GitHub