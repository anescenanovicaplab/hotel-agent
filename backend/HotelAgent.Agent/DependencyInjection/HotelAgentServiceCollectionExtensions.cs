using Microsoft.Extensions.DependencyInjection;

namespace HotelAgent.Agent.DependencyInjection;

public static class HotelAgentServiceCollectionExtensions
{
    /// <summary>
    /// Registers the hotel agent and its dependencies into the service collection.
    /// </summary>
    public static IServiceCollection AddHotelAgent(this IServiceCollection services)
    {
        // Intentionally empty for now — populated in PBI #6 when we add the agent itself.
        return services;
    }
}