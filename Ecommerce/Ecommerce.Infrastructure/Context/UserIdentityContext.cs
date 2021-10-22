using Ecommerce.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Context
{
    public class UserIdentityContext : IdentityDbContext
    {
        public UserIdentityContext(DbContextOptions  options):base(options)
        {

        }

    
    }
}