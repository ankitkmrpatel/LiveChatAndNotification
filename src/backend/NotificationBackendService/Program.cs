using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using NotificationBackendService.Extentions.ServiceCollection;
using NotificationBackendService.Middleware;
using NotificationBackendService.Services;
using NotificationBackendService.Services.Worker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCssMinification();
builder.Services.AddAuthenticationWithJwt(builder.Configuration);

builder.Services.AddSqlLiteDatabase();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICurrentUserIdentity, CurrentUserIdentity>();

builder.Services.AddRepository();
builder.Services.AddApplicationServices();

builder.Services.AddHostedService<NotificationEventSubscriber>();
builder.Services.AddHostedService<NotificationEventSender>();
builder.Services.AddSignalRAndHubs();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
    builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithOrigins("http://localhost:3000");
}));

var app = builder.Build();
app.UseExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCssMinification();
app.UseFileServer();

app.UseHttpsRedirection();

app.AddApplicationAuth();

app.AddEndpoints();
app.UseCors("CorsPolicy");

app.AddSignalRHubs();

app.PrepDbPopulation();

app.Run();