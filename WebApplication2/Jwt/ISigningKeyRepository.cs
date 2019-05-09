using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public interface ISigningKeyRepository
    {
        bool TryGet(string id, out SecurityKey securityKey);
    }
}