// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using Swiss.FCh.Cube.RawData.Contract;
using Swiss.FCh.Cube.RawData.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Swiss.FCh.Cube.RawData.Extensions
{
    /// <summary>
    /// This class holds extension methods that register the services of this library in your DI container.
    /// </summary>
    public static class BkCubeRawDataServiceCollectionExtensions
    {
        /// <summary>
        /// This method registers the services of this library in your DI container.
        /// </summary>
        /// <param name="services">Your <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/></param>
        /// <returns>The <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/> for further use with the fluent syntax.</returns>
        public static IServiceCollection AddRawDataService(this IServiceCollection services)
        {
            services.AddScoped<ICubeRawDataService, RawDataService>();

            return services;
        }
    }
}
