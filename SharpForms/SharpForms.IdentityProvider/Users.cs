using System.Security.Claims;
using Duende.IdentityServer.Test;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Seeds;
using SharpForms.Common.Enums;

namespace SharpForms.IdentityProvider;

public class Users
{
    public static string DefaultPassword { get; } = "password";
    private static IList<TestUser>? _users = null;

    public static List<TestUser> GetUsers()
    {
        if (_users == null)
        {
            _users = TransformSeededUsers();
        }

        return _users.ToList();
    }

    private static IList<TestUser> TransformSeededUsers()
    {
        return UserSeeds.SeededUsers.Select(su => new TestUser
        {
            SubjectId = su.Id.ToString(),
            Username = su.Name.ToLower().Replace(" ", ""),
            Password = DefaultPassword,
            Claims = new List<Claim> { new Claim("role", su.Role.ToString()), new Claim("name", su.Name), }
        }).ToList();
    }

    public static bool ValidateUser(string username, string password)
    {
        var user = GetUsers().FirstOrDefault(u => u.Username == username);
        if (user == null) return false;
        return user.Password == password;
    }
}
