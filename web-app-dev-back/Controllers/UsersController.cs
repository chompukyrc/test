using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using web_app_dev_back.Models;
using web_app_dev_back.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace web_app_dev_back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService) =>
            _usersService = usersService;

        public async Task<List<UserModel>> Get(){
            return await _usersService.GetAllUserAsync();
        }

        [Authorize]
        [HttpGet]
        [Route("Profile")]
        public async Task<UserModel?> GetProfile()
        {
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            Console.WriteLine(userId);
            var user = await _usersService.GetProfileByIdAsync(userId);
            user.Password = "";
            return user;
        }

        [Authorize]
        [HttpGet]
        [Route("ById")]
        public async Task<UserModel> Get(string id)
        {
            return await _usersService.GetAsync(id);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Post(NewUserModel newUser)
        {
            try
            {
                await _usersService.RegisterAsync(newUser);
                return CreatedAtAction("", new { id = "" }, newUser);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginHandle(LoginModel userData)
        {
            var currentUser = await _usersService.LoginAsync(userData);
            if (currentUser is null)
            {
                return NotFound();
            }
            else
            {
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserId", currentUser.Id.ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWT_KEY_SECURE_123"));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    "test",
                    "test",
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(3600),
                    signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile(UpdatedUserProFileModel updatedUser){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var user = await _usersService.GetAsync(userId);
            if (user == null){
                return NotFound();
            }
            user.Username = updatedUser.Username;
            user.Firstname = updatedUser.Firstname;
            user.Lastname = updatedUser.Lastname;
            user.Phone = updatedUser.Phone;
            await _usersService.UpdateAsync(userId,user);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword(UpdatedUserPasswordModel updatedUser){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var user = await _usersService.GetAsync(userId);
            var hashOldPassword = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(updatedUser.OldPassword)).Select(s => s.ToString("x2")));
            if(hashOldPassword!=user.Password){
                return NotFound();
            }
            var hashNewPassword = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(updatedUser.NewPassword)).Select(s => s.ToString("x2")));
            user.Password = hashNewPassword;
            await _usersService.UpdateAsync(userId,user);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateToPremiumUser")]
        public async Task<IActionResult> UpdateToPremiumUser(){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var user = await _usersService.GetAsync(userId);
            if(user==null){return NotFound();}
            user.PremiumMember = true;
            await _usersService.UpdateAsync(userId,user);
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateToNormalUser")]
        public async Task<IActionResult> UpdateToNormalUser(){
            string userId = Request.HttpContext.User.FindFirstValue("UserId");
            var user = await _usersService.GetAsync(userId);
            if(user==null){return NotFound();}
            user.PremiumMember = false;
            await _usersService.UpdateAsync(userId,user);
            return Ok();
        }
    }
}
