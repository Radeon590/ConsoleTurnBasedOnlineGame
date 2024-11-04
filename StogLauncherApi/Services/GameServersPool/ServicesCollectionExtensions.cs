namespace StogLauncherApi.Services.GameServersPool;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddGameServersPool(this IServiceCollection services)
    {
        services.AddSingleton<GameServersPool>();
        return services;
    }
}