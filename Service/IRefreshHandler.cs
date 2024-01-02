namespace restaurant_app_API.Service
{
    public interface IRefreshHandler
    {

        Task<string> GenerateToken(int userId);



    }
}
