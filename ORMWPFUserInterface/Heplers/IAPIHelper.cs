using ORMWPFUserInterface.Models;
using System.Threading.Tasks;

namespace ORMWPFUserInterface.Heplers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}