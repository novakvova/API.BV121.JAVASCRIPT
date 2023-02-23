using Microsoft.AspNetCore.Identity;

namespace WebShop.Models
{
    public class UserItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class UserCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
