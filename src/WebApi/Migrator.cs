using System.Threading.Tasks;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    public class Migrator
    {
        private readonly AppIdentityDbContext _appIdentityDbContext;

        public Migrator(AppIdentityDbContext appIdentityDbContext)
        {
            _appIdentityDbContext = appIdentityDbContext;
        }

        public async Task Run()
        {
            await _appIdentityDbContext.Database.MigrateAsync();
        }
    }
}