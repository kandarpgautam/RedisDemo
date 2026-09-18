using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;

public class AppRunner
{
    private readonly IDistributedCache _cache;
    public AppRunner(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task RunAsync()
    {
        string cacheKey = "myKey";
        string cacheValue = "Hello, Redis!";

        // Absolute Expiration Relative To Now overrides Absolute Expiration if both are set.
        var cacheConfig = new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10), AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15) };

        Console.WriteLine($"Current Datetime: {DateTimeOffset.Now}.");
        Console.WriteLine($"Absolute Expiration Relative To Now: {cacheConfig.AbsoluteExpirationRelativeToNow}.");
        Console.WriteLine($"Absolute Expiration: {cacheConfig.AbsoluteExpiration}");
        await _cache.SetStringAsync(cacheKey, cacheValue, cacheConfig);

        
        foreach (var i in Enumerable.Range(0, 20))
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            string retrievedValue = await _cache.GetStringAsync(cacheKey) ?? "";
            Console.WriteLine($"{i+1}. Retrieved value from cache: {retrievedValue}. Elapsed Time: {stopwatch.Elapsed.Milliseconds.ToString($"{0}ms")}");
            Task.Delay(1000).Wait();
            stopwatch.Stop();
        }

        Console.ReadKey();
    }
}