using System.Collections.Immutable;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using postgreanddotnet.Data;
using restaurant_app_API.Model;
using restaurant_app_API.Service;
using restaurant_app_API.Entity;
using restaurant_app_API.Helper;

namespace restaurant_app_API.Container
{
    public class UserService : IUserService
    {
        private readonly AppDbContex context;
        private readonly IMapper mapper;

        private readonly APIResponse response;
        public UserService(AppDbContex context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.response = new APIResponse(false, "");
        }

        public Task<UserModel> Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse> Create(UserModel user)
        {
            var isUserName = this.context.Users.Where(x => x.Username == user.Username).FirstOrDefault();

            if (isUserName != null)
            {
                response.Message = "Username already exist";
                response.Success = false;
                return Task.FromResult(response);
            }

            try
            {
                var data = this.mapper.Map<User>(user);
                this.context.Users.Add(data);
                this.context.SaveChanges();
                response.Message = "Success";
                response.Success = true;
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return Task.FromResult(response);
            }

        }

        public Task Delete(int id)
        {
            try
            {
                var data = this.context.Users.Where(x => x.Id == id).FirstOrDefault();
                if (data != null)
                {
                    this.context.Users.Remove(data);
                    this.context.SaveChanges();
                }
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<List<UserModel>> GetAll()
        {

            List<UserModel> users = new List<UserModel>();
            var data = await this.context.Users.ToListAsync();
            if (data != null)
            {
                // foreach (var item in data)
                // {
                //     users.Add(this.mapper.Map<UserModel>(item));
                // }
                users = this.mapper.Map<List<User>, List<UserModel>>(data);
            }
            return users;
        }

        public Task<APIResponse> GetById(int id)
        {

            var data = this.context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (data != null)
            {
                var user = this.mapper.Map<UserModel>(data);
                return Task.FromResult(new APIResponse(true, "", user));
            }
            return Task.FromResult(new APIResponse(false, "User not found"));
        }


        public Task<APIResponse> Update(UserModel user, string password = null)
        {
            try
            {
                var data = this.context.Users.Where(x => x.Id == user.Id).FirstOrDefault();
                if (data != null)
                {
                    data.Username = user.Username;
                    data.Password = user.Password;
                    data.Role = user.Role;
                    data.IsActive = user.IsActive;
                    this.context.SaveChanges();
                }
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return Task.FromResult(response);
            }
        }

        Task<APIResponse> IUserService.Authenticate(string username, string password)
        {
            try
            {
                var data = this.context.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();

                if (data != null)
                {
                    var user = this.mapper.Map<UserModel>(data);
                    return Task.FromResult(new APIResponse(true, "", user));
                }
                return Task.FromResult(new APIResponse(false, "User not found"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new APIResponse(false, ex.Message));
            }

        }


    }
}
