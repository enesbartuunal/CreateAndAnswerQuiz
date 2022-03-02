using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using CreateAndAnswerQuiz.Api.DataAccess.Entities;
using CreateAndAnswerQuiz.Business.Base;
using CreateAndAnswerQuiz.Models.Models;

namespace CreateAndAnswerQuiz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
    

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
           
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            if (model is null || !ModelState.IsValid)
                return BadRequest();
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest();
            }
            var user = new User()
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user,"Member");

            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description);
                return BadRequest();
            }
                
            return StatusCode(201);
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var userRoles = _userManager.GetRolesAsync(user);
            userRoles.Wait();
            var userRole = "";
            if (userRoles.Result.Contains("Admin"))
                userRole = "Admin";
            else
                userRole = "Member";
            if (user is null)
                return Unauthorized();
            var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, false, true);
            if (result.IsLockedOut) {
               
                return Unauthorized();
            }
            else if (!result.Succeeded)
                return Unauthorized();

            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            var returnData = new SignInResponseModel()
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
                RefreshToken = user.RefreshToken
            };

            return Ok(returnData);
        }

        [HttpPost("getuserid")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserId([FromBody] string userName)
        {
            if (userName == null)
                return BadRequest();
            var userExists = await _userManager.FindByEmailAsync(userName);
            return Ok(userExists.Id);

        }


    }
}
