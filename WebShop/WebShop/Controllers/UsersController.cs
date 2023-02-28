using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Data.Entities;
using WebShop.Models;

namespace WebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private static readonly List<UserItemViewModel> _users= new ();
        private readonly AppEFContext _context;

        public UsersController(AppEFContext context)
        {
            _context= context;
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var list = _context.Users
                .Select(x=>new UserItemViewModel
                {
                    Id= x.Id,
                    Name = x.Name,
                    Image= x.Image,
                    Description= x.Description,
                })
                .ToList();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserCreateViewModel model)
        {
            string imageName = String.Empty;
            if (model.Image != null)
            {
                string exp = Path.GetExtension(model.Image.FileName);
                imageName = Path.GetRandomFileName() + exp;
                string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                using (var stream = System.IO.File.Create(dirSaveImage))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }
            var user = new UserEntity
            {
                Name = model.Name,
                Description = model.Description,
                Image = imageName,
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }
    }
}
