using GSA.Security.Models;

namespace GSA.Security
{
    public interface IMembershipService
    {
        AuthStatus ValidateUser(string userName, string password);
        string[] GetRoles(string userName);
    }
}
