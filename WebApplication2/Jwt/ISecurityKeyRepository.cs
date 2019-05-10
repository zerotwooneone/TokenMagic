using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public interface ISecurityKeyRepository
    {
        bool TryGet(string id, out SecurityKey securityKey);
    }
}