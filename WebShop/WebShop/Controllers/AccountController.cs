using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebShop.Abastract;
using WebShop.Constants;
using WebShop.Data.Entities.Identity;
using WebShop.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<UserEntity> _userManager;
        public AccountController(IJwtTokenService jwtTokenService, UserManager<UserEntity> userManager)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
        }

        [HttpPost("google/login")]
        public async Task<IActionResult> GoogleLogin([FromForm]GoogleLoginViewModel model)
        {
            var payload = await _jwtTokenService.VerifyGoogleToken(model.Token);
            if(payload == null) {
                return BadRequest();
            }
            string provider = "Google";
            var info = new UserLoginInfo(provider, payload.Subject, provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if(user == null)
                {
                    string exp = Path.GetExtension(model.Image.FileName);
                    var imageName = Path.GetRandomFileName() + exp;
                    string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                    using (var stream = System.IO.File.Create(dirSaveImage))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    user = new UserEntity
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Image = imageName
                    };
                    var resultCreate = await _userManager.CreateAsync(user);
                    if(!resultCreate.Succeeded) {
                        return BadRequest();
                    }
                    await _userManager.AddToRoleAsync(user, Roles.User);
                }
                var resultUserLogin = await _userManager.AddLoginAsync(user, info);
                if (!resultUserLogin.Succeeded)
                {
                    return BadRequest();
                }
            }

            //Тепер можемо користувача логінити
            var token = _jwtTokenService.CreateToken(user);
            return Ok(new { token });

        }

    }
}
