using Core.ActionResults;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<int>, IRole
    {
        public BaseActionResult SetName(string name)
        {
            Name = name;
            return new BaseActionResult((int) Core.Constants.ActionStatuses.Success);
        }
    }
}