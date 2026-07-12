using Domain;
using System.Security.Claims;

namespace Server.Factories
{
    public class ClaimFactory
    {
        public ClaimsPrincipal CreateClaims(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
