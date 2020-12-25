using System;
using ApplicationCore.Entities;
using Core.ActionResults;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IUser
    {
        private ApplicationUser() : base()
        {
        }

        public ApplicationUser(string email, string name) : base()
        {
            Email = email;
            Name = name;
        }
  
        public string Name { get; private set; }


        public string RefreshToken { get; private set; }


        public void SetName(string name)
        {
            Name = name;
        }

        

        public BaseActionResult SetEmail(string email)
        {
            Email = email;
            return new BaseActionResult((int) Core.Constants.ActionStatuses.Success);
        }

        public BaseActionResult SetUserName(string userName)
        {
            base.UserName = userName;
            return new BaseActionResult((int) Core.Constants.ActionStatuses.Success);
        }

        public BaseActionResult SetRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            return new BaseActionResult((int) Core.Constants.ActionStatuses.Success);
        }

    }
}