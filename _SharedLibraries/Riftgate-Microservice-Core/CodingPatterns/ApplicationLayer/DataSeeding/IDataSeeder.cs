namespace Application._Registration.DataSeeder;

public interface IDataSeeder
{
    Task  SeedData(IServiceProvider serviceProvider);
}