namespace WebShop.Models
{
    public class GoogleLoginViewModel
    {
        public string Token { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public IFormFile Image { get; set; }
    }
}
