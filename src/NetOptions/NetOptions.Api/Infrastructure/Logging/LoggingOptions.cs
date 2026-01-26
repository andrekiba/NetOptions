namespace NetOptions.Api.Infrastructure.Logging;

public sealed class LoggingOptions
{
    /// <summary>
    /// The logging configuration section name. This is exposed by convention to 
    /// allow consumers of these options to bind configuration to their named-section.
    /// </summary>
    public const string Name = "Logging";
    
    /// <summary>
    /// Gets or sets the <see cref="LogLevelOptions"/> 
    /// value as represented in the underlying configuration.
    /// </summary>
    /// <remarks>
    /// For example, consider the following JSON appsettings file:
    /// <code language="json">
    /// {
    ///   "Logging": {
    ///     "LogLevel": {
    ///       "Default": "Information",
    ///       "Microsoft.AspNetCore": "Warning"
    ///     }
    ///   },
    ///   "AllowedHosts": "*"
    /// }
    /// </code>
    /// This JSON would populate our log level 
    /// to namespace options.
    /// </remarks>
    public LogLevelOptions LogLevel { get; set; } = new();
}