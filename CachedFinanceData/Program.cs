var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFusionCache()
    .WithSerializer(
        new FusionCacheNewtonsoftJsonSerializer()
    )
    .WithDistributedCache(
        new RedisCache(new RedisCacheOptions
        {
            Configuration = builder.Configuration.GetConnectionString("Redis")
        })
    );
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/", Finance.GetData);
app.MapGet("/car-tax", CarTax.GetData);

app.Run();
