using Microsoft.Extensions.Options;
using NetOptions.Api.Infrastructure.Features;
using NetOptions.Api.Infrastructure.Logging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

#region AddOptions

// Bind IOptions<LoggingOptions> to the
// "Logging" config section for DI.
builder.Services.AddOptions<LoggingOptions>()
    .Bind(config: builder.Configuration.GetSection(key: LoggingOptions.Name));

// Add named configuration options.
builder.Services.AddOptions<FeatureOptions>(name: "PreorderBike")
    .Bind(config: builder.Configuration.GetSection(key: "Features:PreorderBike"));

builder.Services.AddOptions<FeatureOptions>(name: "CustomizeBike")
    .Bind(config: builder.Configuration.GetSection(key: "Features:CustomizeBike"));

#endregion

#region ConfigureOptions
/*
builder.Services.Configure<FeatureOptions>(name: "PreOrderBike",
    config: builder.Configuration.GetSection(key: "Features:PreOrderBike"));

builder.Services.Configure<FeatureOptions>(name: "CustomizeBike",
    config: builder.Configuration.GetSection(key: "Features:CustomizeBike"));
*/
#endregion

#region PostConfigureOptions

// Overrides (and/or merges) with existing configured bindings.
builder.Services.PostConfigure<FeatureOptions>(
    name: "CustomizeBike",
    configureOptions: static (FeatureOptions options) =>
    {
        options.Version = "1.0";
        options.Endpoint = new Uri("https://crazybike.com/api/v1/customize");
        options.Tags =
        [
            "customization"
        ];
    });

// Override all config-bound instances of FeatureOptions.
/*
builder.Services.PostConfigureAll<FeatureOptions>(
    configureOptions: 
    static (FeatureOptions options) => 
        options.Tags ??= []);
*/
#endregion 

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/logging/options",
    static (IOptions<LoggingOptions> options) =>
    {
        var loggingOptions = options.Value;
        return Results.Json(
            data: loggingOptions);
    })
    .WithName("GetLoggingOptions");

app.MapGet("/feature/options",
    static (IOptionsSnapshot<FeatureOptions> options) =>
    {
        var preorder = options.Get("PreorderBike");
        var customize = options.Get("CustomizeBike");

        return Results.Json(
            data: new
            {
                Preorder = preorder,
                Customize = customize
            });
    })
    .WithName("GetFeatureOptions");

app.Run();