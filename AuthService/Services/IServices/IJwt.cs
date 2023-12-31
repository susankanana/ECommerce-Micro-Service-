using AuthService.Models;

namespace AuthService.Services.IServices
{
    public interface IJwt
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> Roles);
    }
}
