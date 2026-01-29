using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// There's some debate over this namespace
//Microsoft.Extensions.DependencyInjection
namespace NetOptions.Library;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds tool services to the given <paramref name="services"/> collection.
    /// Configures the given <paramref name="toolConfigSection"/> to 
    /// the <see cref="ToolOptions"/> object.
    /// <param name="services">
    /// The service collection to add services to.
    /// </param>
    /// <param name="toolConfigSection">
    /// The tool configuration section to bind existing to.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance with other services added.
    /// </returns>
    /// </summary>
    public static IServiceCollection AddToolServices(
        this IServiceCollection services,
        IConfigurationSection toolConfigSection)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(toolConfigSection);

        services.AddOptionsWithValidateOnStart<ToolOptions>()
            .BindConfiguration(toolConfigSection.Key)
            .ValidateDataAnnotations();

        // TODO:
        //   Add tool services to the service collection.

        return services;
    }

    /// <summary>
    /// Adds tool services to the given <paramref name="services"/> collection.
    /// Then add existing for the<see cref="ToolOptions"/> type,
    /// and binds against the configuration with the <paramref name="configSectionPath"/>.
    /// <param name="services">
    /// The service collection to add services to.
    /// </param>
    /// <param name="configSectionPath">
    /// The tool configuration section to bind existing from.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance with other services added.
    /// </returns>
    /// </summary>
    public static IServiceCollection AddToolServices(
        this IServiceCollection services,
        string configSectionPath)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configSectionPath);

        services.AddOptionsWithValidateOnStart<ToolOptions>()
            .BindConfiguration(configSectionPath)
            .ValidateDataAnnotations();

        // TODO:
        //   Add tool services to the service collection.

        return services;
    }

    /// <summary>
    /// Adds tool services to the given <paramref name="services"/> collection.
    /// Post configures the <paramref name="toolOptions"/> as the source of 
    /// truth, overriding all other previously configured values.
    /// <param name="services">
    /// The service collection to add services to.
    /// </param>
    /// <param name="toolOptions">
    /// The tool existing instance to bind to.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance with other services added.
    /// </returns>
    /// </summary>
    public static IServiceCollection AddToolServices(
        this IServiceCollection services,
        ToolOptions toolOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(toolOptions);

        // TODO:
        //   Determine if you want to use:
        //      .Configure(configureOptions);     // Runs before PostConfigure calls
        //   Or instead use:
        //      .PostConfigure(configureOptions); // Runs after all Configure calls

        services.AddOptionsWithValidateOnStart<ToolOptions>()
            .PostConfigure(configureOptions: existing =>
            {
                // Overwrite existing values with
                // user-provided values.
                existing.Color = toolOptions.Color;
                existing.ImageUrl = toolOptions.ImageUrl;
                existing.IsEnabled = toolOptions.IsEnabled;
                existing.Size = toolOptions.Size;
            })
            .ValidateDataAnnotations();

        // TODO:
        //   Add tool services to the service collection.

        return services;
    }

    /// <summary>
    /// Adds tool services to the given <paramref name="services"/> collection.
    /// Configures the <paramref name="configureOptions"/> to 
    /// the <see cref="ToolOptions"/> object.
    /// <param name="services">
    /// The service collection to add services to.
    /// </param>
    /// <param name="configureOptions">
    /// The tool configuration section to bind existing from.
    /// </param>
    /// <returns>
    /// The same <paramref name="services"/> instance with other services added.
    /// </returns>
    /// </summary>
    public static IServiceCollection AddToolServices(
        this IServiceCollection services,
        Action<ToolOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        // TODO:
        //   Determine if you want to use:
        //      .Configure(configureOptions);     // Runs before PostConfigure calls
        //   Or instead use:
        //      .PostConfigure(configureOptions); // Runs after all Configure calls

        services.AddOptionsWithValidateOnStart<ToolOptions>()
            .Configure(configureOptions)
            .ValidateDataAnnotations();

        // TODO:
        //   Add tool services to the service collection.

        return services;
    }
}
