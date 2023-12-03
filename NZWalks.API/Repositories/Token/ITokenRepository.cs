using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories.Token
{
    public interface ITokenRepository
    {
         string CreateJWTToken(IdentityUser user,List<string> roles);
    }
}
