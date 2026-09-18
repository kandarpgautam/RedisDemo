using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var appSettings = File.ReadAllBytes("appsettings.json");

IServiceCollection services = new ServiceCollection()
    .AddStackExchangeRedisCache(options =>
    {
        options.Configuration = configuration.GetSection("RedisEndpoint").Value;
        options.InstanceName = "RedisDemo";
    }).AddTransient<AppRunner>();

IServiceProvider serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<AppRunner>();
await app.RunAsync();