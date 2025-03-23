namespace LowlandTech.Core.Abstractions;

public interface IFramework
{
    IServiceProvider Services { get; }
    IConfiguration Configuration { get; }
}
