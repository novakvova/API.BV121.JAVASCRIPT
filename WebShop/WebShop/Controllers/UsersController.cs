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
        public IActionResult Create(UserCreateViewModel model)
        {
            var user = new UserEntity
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

    }
}
