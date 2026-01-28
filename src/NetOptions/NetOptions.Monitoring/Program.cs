using NetOptions.Monitoring.TemperatureStation;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<TemperatureStationFactory>();
builder.Services.AddHostedService<TemperatureMonitorWorker>();

builder.Services.AddOptionsWithValidateOnStart<TemperatureStationOptions>()
    .BindConfiguration(configSectionPath: 
        TemperatureStationOptions.Name)
    .ValidateDataAnnotations();

var host = builder.Build();
host.Run();