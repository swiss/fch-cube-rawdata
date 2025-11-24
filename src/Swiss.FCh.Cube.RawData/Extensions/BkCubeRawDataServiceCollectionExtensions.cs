// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using Swiss.FCh.Cube.RawData.Contract;
using Swiss.FCh.Cube.RawData.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Swiss.FCh.Cube.RawData.Extensions
{
    public static class BkCubeRawDataServiceCollectionExtensions
    {
        public static IServiceCollection AddRawDataService(this IServiceCollection services)
        {
            services.AddScoped<ICubeRawDataService, RawDataService>();

            return services;
        }
    }
}
