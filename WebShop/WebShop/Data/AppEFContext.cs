using Microsoft.EntityFrameworkCore;
using WebShop.Data.Entities;

namespace WebShop.Data
{
    public class AppEFContext : DbContext
    {
        public AppEFContext(DbContextOptions<AppEFContext> options)
                : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
