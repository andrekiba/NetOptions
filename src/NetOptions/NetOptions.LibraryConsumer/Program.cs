using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NetOptions.Library;

var builder = Host.CreateApplicationBuilder(args);

#region .AddToolServices(IConfiguration config)

// The Add* API expects an IConfiguration
// that's named "ToolOptions".
//builder.Services.AddToolServices(
//    toolConfigSection: builder.Configuration.GetSection(
//        key: nameof(ToolOptions)));

#endregion

#region .AddToolServices(string configSectionPath)

// The Add* API expects a string that represents the path
// to map to the "ToolOptions" object.
//builder.Services.AddToolServices(
//    configSectionPath: "ToolOptions");

#endregion

#region .AddToolServices(WidgetOptions widgetOptions)

// The Add* API expects a ToolOptions instance.
//builder.Services.AddToolServices(
//    toolOptions: new ToolOptions
//    {
//        Color = "Red",
//        ImageUrl = "https://bit.ly/net-tool",
//        Size = 420,
//        IsEnabled = true,
//    });

#endregion

#region .AddToolServices(Action<WidgetOptions> configureOptions)

// The Add* API expects an options delegate (Action<ToolOptions>)
// that's used to override option values.
builder.Services.AddToolServices(
    configureOptions: static options =>
    {
        options.Color = "Green";
        options.ImageUrl = "https://bit.ly/net-tool";
        options.IsEnabled = false;
        options.Size = 42;
    });

#endregion

var host = builder.Build();

await host.StartAsync();

// TODO:
// Consume available options interfaces:
//   - IOptions<ToolOptions>
//   - IOptionsSnapshot<ToolOptions>
//   - IOptionsMonitor<ToolOptions>
// in a custom registered service.

var options = host.Services.GetRequiredService<IOptions<ToolOptions>>();

var toolOptions = options.Value;

if (toolOptions is not null)
{
}