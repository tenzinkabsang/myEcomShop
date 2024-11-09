using Ecom.QueueProcessor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<OrderPlacedHandler>();

//builder.Services.AddSerilog((sp, loggerConfig) => loggerConfig.ReadFrom.Configuration(builder.Configuration));

//builder.Services.AddSingleton<IEventPublisher, EventPublisher>(sp => new EventPublisher(sp));

var host = builder.Build();
host.Run();
