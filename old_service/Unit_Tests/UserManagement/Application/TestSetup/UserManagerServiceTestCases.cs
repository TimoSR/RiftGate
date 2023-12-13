using Application.DTO.UserManagement;
using Domain.UserManagement.Enums;

namespace Unit_Tests.UserManagement.Application.TestSetup;

public class UserManagerServiceTestCases
{
    public static IEnumerable<object[]> RegistrationTestCases()
    {
        // Successful case
        yield return new object[] { new UserRegisterDto { Email = "validemail@example.com", UserName = "validUser", Password = "ValidPassword1" }, true, 1 };

        // Invalid email
        yield return new object[] { new UserRegisterDto { Email = "invalidemail", UserName = "user", Password = "password" }, false, 0 };

        // User already exists
        yield return new object[] { new UserRegisterDto { Email = "existingemail@example.com", UserName = "existingUser", Password = "ExistingPassword" }, false, 0 };

        // Null values
        yield return new object[] { new UserRegisterDto { Email = null, UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = null, Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "user", Password = null }, false, 0 };

        // Empty strings
        yield return new object[] { new UserRegisterDto { Email = "", UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "user", Password = "" }, false, 0 };

        // Excessively large input
        var longString = new string('a', 10000);
        yield return new object[] { new UserRegisterDto { Email = longString, UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = longString, Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = "user", Password = longString }, false, 0 };
        
        // Simulating negative value in string context
        yield return new object[] { new UserRegisterDto { Email = "negativevalue@example.com", UserName = "-1", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "-1@example.com", UserName = "user", Password = "password" }, false, 0 };

        // Simulating int.MaxValue + 1 in a string context
        yield return new object[] { new UserRegisterDto { Email = $"{int.MaxValue + 1L}@example.com", UserName = "user", Password = "password" }, false, 0 };
        yield return new object[] { new UserRegisterDto { Email = "email@example.com", UserName = $"User{int.MaxValue + 1L}", Password = "password" }, false, 0 };
    }
    
    public static IEnumerable<object[]> GetUserByEmailTestCases()
    {
        yield return new object[] { "foundincache@example.com", true, true, null }; // Found in cache
        yield return new object[] { "foundindb@example.com", true, false, null }; // Not found in cache, but found in DB
        yield return new object[] { "notfoundanywhere@example.com", false, false, null }; // Not found in cache or DB
        yield return new object[] { "error@example.com", false, false, new Exception("Database error") }; // Exception scenario
    }
    
    public static IEnumerable<object[]> DeleteUserByEmailTestCases()
    {
        yield return new object[] { "existinguser@example.com", true, null }; // User exists and is deleted
        yield return new object[] { "nonexistentuser@example.com", false, null }; // User does not exist
        yield return new object[] { "error@example.com", false, new Exception("Database error") }; // Exception scenario
    }

    public static IEnumerable<object[]> UpdateUserStatusByEmailTestCases()
    {
        yield return new object[] { "existinguser@example.com", UserStatus.Active, true, null }; // Successful update
        yield return new object[] { "nonexistentuser@example.com", UserStatus.Active, false, null }; // User does not exist
        yield return new object[] { "error@example.com", UserStatus.Active, false, new Exception("Database error") }; // Exception scenario
    }

    public static IEnumerable<object[]> RollBackUserTestCases()
    {
        yield return new object[] { "existinguser@example.com", true, null }; // Successful rollback
        yield return new object[] { "nonexistentuser@example.com", false, null }; // Rollback fails
        yield return new object[] { "error@example.com", false, new Exception("Database error") }; // Exception scenario
    }


}