using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webApplication.Models;

namespace webApplication.Context
{
    //4th step
    public class DbContextApp : IdentityDbContext<User>
    {
        public DbContextApp(DbContextOptions<DbContextApp> options):base(options) { }
    }
}
