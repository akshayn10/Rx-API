using Microsoft.AspNetCore.Identity;
using Rx.Domain.DTOs.User;

namespace Rx.Domain.Entities.Primary;

public class ApplicationUser:IdentityUser
{
    public string FullName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    public bool OwnsToken(string token)
    {
        return this.RefreshTokens?.Find(x => x.Token == token) != null;
    }
}