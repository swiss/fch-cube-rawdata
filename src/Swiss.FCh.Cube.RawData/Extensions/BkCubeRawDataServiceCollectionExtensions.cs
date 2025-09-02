using Swiss.FCh.Cube.RawData.Contract;
using Swiss.FCh.Cube.RawData.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Swiss.FCh.Cube.RawData.Extensions;

// ReSharper disable once UnusedType.Global : Used by the library's consumers
public static class BkCubeRawDataServiceCollectionExtensions
{
    // ReSharper disable once UnusedMember.Global : Used by the library's consumers
    public static IServiceCollection AddCubeRawData(this IServiceCollection services)
    {
        services.AddScoped<ICubeRawDataService, CubeRawDataService>();

        return services;
    }
}
