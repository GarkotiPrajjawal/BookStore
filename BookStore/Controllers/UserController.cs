using AutoMapper;
using Azure;
using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Repository;
using BookStore.Repository.iRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly iUserRepository _userrepository;
        private string secretKey;
        public UserController(iUserRepository userrepository, IConfiguration configuration)
        {
            _userrepository = userrepository;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers()
        {
            var userslist = await _userrepository.GetAllAsync();
            if (userslist == null)
            {
                return BadRequest();
            }
            return Ok(userslist);
        }
        [HttpGet("{name}", Name = "GetUserByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserbyName(string name)
        {
            if (name == null) { return BadRequest(); }

            User user = await _userrepository.GetAsync(u => u.Name.ToLower() == name.ToLower());
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            User usercheck = await _userrepository.GetAsync(u => u.Name.ToLower() == user.Name.ToLower());
            if (user == null || usercheck != null) { return BadRequest(); }
            await _userrepository.CreateAsync(user);
            return StatusCode(201);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{name}", Name = "DeleteUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(String name)
        {
            if (name == null) { return BadRequest(); }
            User usercheck = await _userrepository.GetAsync(u => u.Name.ToLower() == name.ToLower());
            if (usercheck == null) { return NotFound(); }
            await _userrepository.RemoveAsync(usercheck);
            return Ok();
        }

        [HttpPut("{name}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(String name, [FromBody] User user)
        {
            if (user == null || (name.ToLower() != user.Name.ToLower())) { return BadRequest(); }
            User usercheck = await _userrepository.GetAsync(u => u.Name.ToLower() == name.ToLower(), false);
            if (usercheck == null) { return BadRequest(); }
            await _userrepository.UpdateAsync(user);
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            User namecheck = await _userrepository.GetAsync(u => u.Name.ToLower() == model.Name.ToLower());
            User passwordcheck = await _userrepository.GetAsync(u => u.Password.ToLower() == model.Password.ToLower());
            if (namecheck == null || namecheck.Password.ToLower() != model.Password.ToLower())
            {
                return BadRequest();
            }
            var roles = namecheck.Role;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, namecheck.Name.ToString()),
                    new Claim(ClaimTypes.Role, roles)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var sendToken = tokenHandler.WriteToken(token);
            return Ok(sendToken);
        }
    }
}
