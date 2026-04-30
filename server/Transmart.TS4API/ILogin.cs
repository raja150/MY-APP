using Refit;
using System.Threading.Tasks;
using Transmart.TS4API.Models;

namespace Transmart.TS4API
{ 
    public interface ILogin
    {
        [Post("/api/v2/login")]
        [Headers("Content-Type: application/json")]
        Task<UserInfoModel> GetUser([Body] LoginModel user);
    }
}