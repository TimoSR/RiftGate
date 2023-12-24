using AutoMapper;
using System.Reflection;

namespace API._DIRegister;

public static class AutoMapperRegister
{
    public static void RegisterAutoMapperProfiles()
    {
        var assembly = Assembly.GetExecutingAssembly(); // Change this to your assembly if needed
        var profileTypes = assembly.GetTypes()
            .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var profileType in profileTypes)
        {
            var profileInstance = Activator.CreateInstance(profileType) as Profile;
            var config = new MapperConfiguration(cfg => cfg.AddProfile(profileInstance));
            var mapper = config.CreateMapper();
            Console.WriteLine($"Registered AutoMapper profile: {profileType.Name}");
        }
    }
}