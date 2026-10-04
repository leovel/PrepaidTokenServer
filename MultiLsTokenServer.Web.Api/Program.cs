using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;
using MultiLsTokenServer.Domain.Interfaces.Commands.Diagnostic;
using MultiLsTokenServer.Domain.Interfaces.Commands.Management;
using MultiLsTokenServer.Domain.Interfaces.Commands.TestDisplay;
using MultiLsTokenServer.Domain.Interfaces.Commands.Vending;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Infrastructure.Commands.Base.Utilities;
using MultiLsTokenServer.Infrastructure.Commands.Diagnostic;
using MultiLsTokenServer.Infrastructure.Commands.Management;
using MultiLsTokenServer.Infrastructure.Commands.TestDisplay;
using MultiLsTokenServer.Infrastructure.Commands.Vending;
using MultiLsTokenServer.Infrastructure.Comunication;

var builder = WebApplication.CreateBuilder(args);

//builder.WebHost.ConfigureKestrel((context, serverOptions) =>
//{
//    serverOptions.ListenAnyIP(8080, o => o.Protocols = HttpProtocols.Http1);
//});

string hsmHost = builder.Configuration["HSM_HOST"] ?? "127.0.0.1";
var hsmPort = builder.Configuration.GetValue<int?>("HSM_PORT") ?? 5100;

// Add services to the container.
builder.Services.AddSingleton<IAsynchronousHSMClient, IAsynchronousHSMClient>(provider => new AsynchronousHSMClient(hsmHost, hsmPort));
builder.Services.AddSingleton<ITidControlService, TidControlService>();

builder.Services.AddScoped<IHsmDiagnosticService, HsmDiagnosticService>();
builder.Services.AddScoped<IHsmVendingElectricityService, HsmVendingElectricityService>();
builder.Services.AddScoped<IHsmVendingWaterService, HsmVendingWaterService>();
builder.Services.AddScoped<IHsmVendingGasService, HsmVendingGasService>();
builder.Services.AddScoped<IHsmVendingTimeService, HsmVendingTimeService>();
builder.Services.AddScoped<IHsmManagementService, HsmManagementService>();
builder.Services.AddSingleton<IMeterTestDisplayService, MeterTestDisplayService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
