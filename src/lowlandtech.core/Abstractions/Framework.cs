namespace LowlandTech.Core.Abstractions;

public class Framework(IServiceProvider services, IConfiguration configuration) : IFramework
{
    public IServiceProvider Services { get; } = services;
    public IConfiguration Configuration { get; } = configuration;
}