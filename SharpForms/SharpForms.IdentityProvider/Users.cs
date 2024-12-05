using System.Security.Claims;
using Duende.IdentityServer.Test;

namespace SharpForms.IdentityProvider;

public class Users
{
    public static string DefaultPassword { get; } = "password";

    public static List<TestUser> GetUsers()
    {
        return new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "admin",
                Password = DefaultPassword,
                Claims = new List<Claim>
                {
                    new Claim("role", "admin"),
                    new Claim("email", "user@example.com"),
                    new Claim("custom_claim", "another_value")
                }
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "user",
                Password = DefaultPassword,
                Claims = new List<Claim> { new Claim("role", "general") }
            }
        };
    }
}
