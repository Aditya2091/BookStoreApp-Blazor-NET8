using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Dtos.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [AllowAnonymous]
   public class AuthController : ControllerBase
   {
      private readonly UserManager<ApiUser> userManager;
      private readonly IConfiguration configuration;

      public ILogger<AuthController> logger { get; }
      public IMapper mapper { get; }

      public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
      {
         this.logger=logger;
         this.mapper=mapper;
         this.userManager=userManager;
         this.configuration=configuration;
      }

      [HttpPost]
      [Route("register")]
      public async Task<IActionResult> Register(UserDto userDto)
      {
         logger.LogInformation($"Registration Attempted for {userDto.Email}");
         try
         {
            var user = mapper.Map<ApiUser>(userDto);
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
               foreach (var error in result.Errors)
               {
                  ModelState.AddModelError(error.Code, error.Description);
               }
               return BadRequest(ModelState);
            }

            await userManager.AddToRoleAsync(user, userDto.Role);
            return Accepted();
         }
         catch (Exception ex) 
         {
            logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
            return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
         }
         
      }

      [HttpPost]
      [Route("login")]
      public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
      {
         logger.LogInformation($"Login Attempted for {userDto.Email}");
         try
         {
            var user = await userManager.FindByEmailAsync(userDto.Email);
            var password = await userManager.CheckPasswordAsync(user, userDto.Password);

            if(user == null || password == false)
            {
               return Unauthorized(userDto);
            }

            string token = await GenerateToken(user);

            var response = new AuthResponse
            {
               Email = userDto.Email,
               Token = token,
               UserId = user.Id
            };

            return response;
         }
         catch (Exception ex)
         {
            logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
            return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
         }
      }

      private async Task<string> GenerateToken(ApiUser user)
      {
         var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

         var roles = await userManager.GetRolesAsync(user);
         var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
         var userClaims = await userManager.GetClaimsAsync(user);

         var claims = new List<Claim>
         {
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(CustomClaimTypes.Uid, user.Id)
         }
         .Union(userClaims)
         .Union(roleClaims);

         var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])),
            signingCredentials: credentials);

         return new JwtSecurityTokenHandler().WriteToken(token);
      }
   }
}
