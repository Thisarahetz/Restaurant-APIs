using restaurant_app_API.Model;
using restaurant_app_API.Helper;

namespace restaurant_app_API.Service
{
    public interface IUserService
    {
        Task<APIResponse> Authenticate(string username, string password);
        Task<List<UserModel>> GetAll();
        Task<APIResponse> GetById(int id);
        Task<APIResponse> Create(UserModel user);
        Task<APIResponse> Update(UserModel user, string id);
        Task Delete(int id);


    }
}
