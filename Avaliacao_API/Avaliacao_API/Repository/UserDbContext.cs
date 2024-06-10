using Avaliacao_API.Entity;
using Microsoft.EntityFrameworkCore;

namespace Avaliacao_API.Repository
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<USERS> Users { get; set; }
    }
}