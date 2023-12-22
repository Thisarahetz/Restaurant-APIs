using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurant_app_API.Model;
using restaurant_app_API.Service;

namespace restaurant_app_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAll();
            if (users == null) return NotFound();
            Console.WriteLine(users);

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserModel user)
        {
            var response = await _userService.Create(user);
            if (response.Success == false) return BadRequest(response.Message);
            return Ok(response.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(UserModel user, string id)
        {
            var response = await _userService.Update(user, id);
            if (response.Success == false) return BadRequest(response.Message);
            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _userService.Delete(id);
            return Ok();
        }

    }
}
