using NetOptions.Validation;
using NetOptions.Validation.Infrastructure.Features;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<FeatureOptions>(name: "PreorderBike")
    .BindConfiguration("Features:PreorderBike")
    //.Bind(config: builder.Configuration.GetSection(key: "Features:PreorderBike"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<FeatureOptions>(name: "CustomizeBike")
    .BindConfiguration("Features:CustomizeBike")
    //.Bind(config: builder.Configuration.GetSection(key: "Features:CustomizeBike"))
    .ValidateDataAnnotations();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();