using MultiLsTokenServer.Grpc.Api.Interceptors;
using MultiLsTokenServer.Grpc.Api.Services;
using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;
using MultiLsTokenServer.Domain.Interfaces.Commands.Diagnostic;
using MultiLsTokenServer.Domain.Interfaces.Commands.Management;
using MultiLsTokenServer.Domain.Interfaces.Commands.TestDisplay;
using MultiLsTokenServer.Domain.Interfaces.Commands.Vending;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Infrastructure.Comunication;
using MultiLsTokenServer.Infrastructure.Commands.Base.Utilities;
using MultiLsTokenServer.Infrastructure.Commands.Diagnostic;
using MultiLsTokenServer.Infrastructure.Commands.Vending;
using MultiLsTokenServer.Infrastructure.Commands.Management;
using MultiLsTokenServer.Infrastructure.Commands.TestDisplay;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

string hsmHost = builder.Configuration["HSM_HOST"] ?? "127.0.0.1";
var hsmPort = builder.Configuration.GetValue<int?>("HSM_PORT") ?? 5100;

//builder.WebHost.ConfigureKestrel((context, serverOptions) =>
//{
//    serverOptions.ListenAnyIP(5443, o => o.Protocols = HttpProtocols.Http2);
//});

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 128 * 1024; // 128 KB
    options.MaxSendMessageSize = 256 * 1024; // 256 KB
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.AddSingleton<ExceptionInterceptor>();

builder.Services.AddSingleton<IAsynchronousHSMClient, IAsynchronousHSMClient>(provider => new AsynchronousHSMClient(hsmHost, hsmPort));
builder.Services.AddSingleton<ITidControlService, TidControlService>();

builder.Services.AddScoped<IHsmDiagnosticService, HsmDiagnosticService>();
builder.Services.AddScoped<IHsmVendingElectricityService, HsmVendingElectricityService>();
builder.Services.AddScoped<IHsmVendingWaterService, HsmVendingWaterService>();
builder.Services.AddScoped<IHsmVendingGasService, HsmVendingGasService>();
builder.Services.AddScoped<IHsmVendingTimeService, HsmVendingTimeService>();
builder.Services.AddScoped<IHsmManagementService, HsmManagementService>();
builder.Services.AddSingleton<IMeterTestDisplayService, MeterTestDisplayService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiagnosticService>();
app.MapGrpcService<VendingElectricityService>();
app.MapGrpcService<VendingWaterService>();
app.MapGrpcService<VendingGasService>();
app.MapGrpcService<VendingTimeService>();
app.MapGrpcService<ManagementService>();
app.MapGrpcService<TestDisplayService>();

app.MapGet("/", () => "All Services Running.");

app.Run();
