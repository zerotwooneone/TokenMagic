using Microsoft.IdentityModel.Tokens;

namespace WebApplication2.Jwt
{
    public interface ISigningCredentialRepository
    {
        bool TryGet(string id, out SigningCredentials signingCredentials);
    }
}