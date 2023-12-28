using API._DIRegister;
using API.Features._shared.API;
using AspNetCoreRateLimit;
using Infrastructure._DIRegister;
using Infrastructure.Configuration;
using Infrastructure.Persistence.Google_PubSub;
using Infrastructure.Persistence.MongoDB;
using Infrastructure.Persistence.Redis;
using Infrastructure.Swagger;

namespace API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DotNetEnv.Env.Load();

        var hostUrl = DotNetEnv.Env.GetString("HOST_URL");
        var serviceName = DotNetEnv.Env.GetString("SERVICE_NAME");
        var projectId = DotNetEnv.Env.GetString("GOOGLE_CLOUD_PROJECT");
        var environment = DotNetEnv.Env.GetString("ENVIRONMENT");
        var mongoConnectionString = DotNetEnv.Env.GetString("MONGODB_CONNECTION_STRING");
        var redisConnectionString = DotNetEnv.Env.GetString("REDIS_CONNECTION_STRING");
        var jwtKey = DotNetEnv.Env.GetString("JWT_KEY");
        var jwtAudience = DotNetEnv.Env.GetString("JWT_AUDIENCE");
        var envVars = Environment.GetEnvironmentVariables();
        
        //Adding all settings to the config

        ICustomConfiguration config = new CustomConfiguration()
        {
            HostUrl = hostUrl,
            ProjectId = projectId,
            ServiceName = serviceName,
            Environment = environment,
            MongoConnectionString = mongoConnectionString,
            RedisConnectionString = redisConnectionString,
            EnvironmentVariables = envVars,
            JwtKey = jwtKey,
            JwtIssuer = serviceName,
            JwtAudience = jwtAudience
        };

        builder.Services.AddSingleton(config);

        Console.WriteLine($"\n{serviceName}");
        Console.WriteLine("\n###################################\n");

        // Custom Tools written tools to simplify development
        builder.Services.RegisterInfrastructureServices();
        
        // Add / Disable MongoDB
        builder.Services.AddMongoDbServices(config);
        // Add / Disable Publisher
        builder.Services.AddPublisherClient(config);
        // Add / Disable Subscriber 
        builder.Services.AddSubscriberClient();
        // Add / Disable Redis
        builder.Services.AddRedisServices(config);

        // Adding All Pub & Sub Events with reflection
        builder.Services.AddSingleton<SubTopicsRegister>();
        builder.Services.AddSingleton<PubTopicsRegister>();

        // Hosting to make sure it dependencies connect on Program startup
        builder.Services.AddHostedService<StartPersistenceConnections>();
        
        // Implemented but should not be enabled by default
        // Currently the checks runs each minute, but could be modified with env var. 
        // so could be enabled in environment or feature flags
        //builder.Services.AddHostedService<AuctionExpiryBackgroundService>();
        
        // Adding Database Repositories
        builder.Services.AddApplicationRepositories();

        // Add this after all project dependencies to register all the services.
        builder.Services.AddDomainServices();
        builder.Services.AddCommandHandlers();
        builder.Services.AddQueryHandlers();
        
        //Security

        //builder.Services.AddAuthentication();
        //builder.Services.AddAuthorization();
        
        // Add / Disable GraphQL (MapGraphQL should be out-commented too)
        //builder.Services.AddGraphQlServices();
        
        //Adding the Controllers
        builder.Services.AddControllers();
        
        //Adding Mediator
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        
        //Adding Automapper
        builder.Services.AddAutoMapper(typeof(Program));
        AutoMapperRegister.RegisterAutoMapperProfiles();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerServices();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyCorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin() // or specify the allowed origins
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        
        // Add memory cache services
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        
        // Adding Rate Limiting 
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.GenerateSwaggerDocs();
        }
        
        // Enable this for Https only
        //app.UseHttpsRedirection();
    
        // Controller Middlewares
        app.UseCors("MyCorsPolicy");
        //app.UseMiddleware<RequestLoggingMiddleware>();
        //app.UseIpRateLimiting();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseAuthentication();
        //app.UseAuthorization();

        app.MapControllers();

        // Websockets is required to enable subscriptions with GraphQL
        app.UseWebSockets();

        //app.MapGraphQL();

        await app.RunAsync();
    }
}

    // if (environment.Equals("Development")) {
    //
    //     // Insert initial data into the MongoDB collections
    //
    //     var seederType = typeof(IDataSeeder);
    //     var seeders = AppDomain.CurrentDomain.GetAssemblies()
    //         .SelectMany(s => s.GetTypes())
    //         .Where(p => seederType.IsAssignableFrom(p) && !p.IsInterface)
    //         .ToList();
    //
    //     foreach(var seeder in seeders)
    //     {
    //         var instance = Activator.CreateInstance(seeder) as IDataSeeder;
    //         instance?.SeedData(app.Services);
    //     }
    //     
    //     Console.WriteLine("\n###################################");
    //     Console.WriteLine("\nSeeding Database due to ENV: Development...");
    // }


    // The standard way of implementing jwt auth in dotnet
    // Tried to implement my own method of handling it.
    // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //     .AddJwtBearer(options =>
    //     {
    //         options.TokenValidationParameters = new TokenValidationParameters
    //         {
    //             ValidateIssuer = true,
    //             ValidateAudience = true,
    //             ValidateLifetime = true,
    //             ValidateIssuerSigningKey = true,
    //             ValidIssuer = config.JwtIssuer,
    //             ValidAudience = config.JwtAudience,
    //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtKey))
    //         };
    //     });

    //app.UseAuthentication(); // This is the disabled auth.