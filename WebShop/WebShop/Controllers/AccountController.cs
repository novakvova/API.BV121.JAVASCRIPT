using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebShop.Abastract;
using WebShop.Constants;
using WebShop.Data.Entities.Identity;
using WebShop.Models;
using WebShop.Services;
using static System.Net.Mime.MediaTypeNames;

namespace WebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ISmtpEmailService _emailService;
        private readonly IConfiguration _configuration;
        public AccountController(IJwtTokenService jwtTokenService, 
            UserManager<UserEntity> userManager, 
            ISmtpEmailService emailService, 
            IConfiguration configuration)
        {
            _jwtTokenService = jwtTokenService;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("google/login")]
        public async Task<IActionResult> GoogleLogin(GoogleLogInViewModel model)
        {
            var payload = await _jwtTokenService.VerifyGoogleToken(model.Token);
            var token = "";
            if (payload == null)
            {
                return BadRequest();
            }
            string provider = "Google";
            var info = new UserLoginInfo(provider, payload.Subject, provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            bool isNewUser = false;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    isNewUser = true;

                    IFormFile ifile = await ConvertUrlToFormFile.ConvertUrlToIFormFile(model.ImagePath);

                    var imageName = Path.GetRandomFileName() + ".jpg";
                    string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                    using (var stream = System.IO.File.Create(dirSaveImage))
                    {
                        await ifile.CopyToAsync(stream);
                    }

                    user = new UserEntity
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Image = imageName

                    };
                    return Ok(new { isNewUser, user, token });

                }

                var resultuserLogin = await _userManager.AddLoginAsync(user, info);
                if (!resultuserLogin.Succeeded)
                {
                    return BadRequest();
                }
            }

            token = await _jwtTokenService.CreateToken(user);

            return Ok(new { isNewUser, user, token });
        }

        [HttpPost("google/registartion")]
        public async Task<IActionResult> GoogleRegistartion([FromForm] GoogleLogInViewModel model)
        {
            var payload = await _jwtTokenService.VerifyGoogleToken(model.Token);
            if (payload == null)
            {
                return BadRequest();
            }
            string provider = "Google";
            var info = new UserLoginInfo(provider, payload.Subject, provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    var imageName = "";
                    if (model.UploadImage != null)
                    {
                        string exp = Path.GetExtension(model.UploadImage.FileName);
                        imageName = Path.GetRandomFileName() + exp;
                        string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                        using (var stream = System.IO.File.Create(dirSaveImage))
                        {
                            await model.UploadImage.CopyToAsync(stream);
                        }
                        model.ImagePath = imageName;
                    }

                    user = new UserEntity
                    {
                        Email = payload.Email,
                        UserName = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Image = model.ImagePath,

                    };
                    var resultCreate = await _userManager.CreateAsync(user);
                    if (!resultCreate.Succeeded)
                    {
                        return BadRequest();
                    }

                    await _userManager.AddToRoleAsync(user, Roles.User);

                    return Ok();

                }
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isPasswordValid)
                {
                    return BadRequest();

                }
                var token = _jwtTokenService.CreateToken(user);
                return Ok(new { token });
            }
            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel model)
        {

            string imageName = String.Empty;
            if (model.UploadImage != null)
            {
                string exp = Path.GetExtension(model.UploadImage.FileName);
                imageName = Path.GetRandomFileName() + exp;
                string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                using (var stream = System.IO.File.Create(dirSaveImage))
                {
                    await model.UploadImage.CopyToAsync(stream);
                }
                model.ImgPath = imageName;
            }

            UserEntity user = new UserEntity()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Image = imageName
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                result = _userManager.AddToRoleAsync(user, Roles.User).Result;
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var frontendUrl = _configuration.GetValue<string>("FrontEndURL");

            var callbackUrl = $"{frontendUrl}/resetpassword?userId={user.Id}&" +
                $"code={WebUtility.UrlEncode(token)}";

            var message = new Message()
            {
                To = user.Email,
                Subject = "Відновлення пароля",
                Body = "Щоб відновити пароль нажміть на посилання:" +
                    $"<a href='{callbackUrl}'>Відновити пароль</a>"
            };
            _emailService.Send(message);
            return Ok();
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var res = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return Ok();
        }

    }
}
