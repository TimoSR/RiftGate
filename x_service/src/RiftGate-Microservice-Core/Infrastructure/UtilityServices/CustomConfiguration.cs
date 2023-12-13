using System.Collections;
using Infrastructure.UtilityServices._Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.UtilityServices;

public class CustomConfiguration : ICustomConfiguration
{
    public string HostUrl { get; set; }
    public string ProjectId { get; set; }
    public string ServiceName { get; set; }
    public string Environment { get; set; }
    public IDictionary EnvironmentVariables { get; set; }
    public string MongoConnectionString { get; set; }
    public string RedisConnectionString { get; set; }
    public string JwtKey { get; set; }
    public string JwtAudience { get; set; }
    public string JwtIssuer { get; set; }
    public RsaSecurityKey? JwtEncryptionKey { get; set; }
}