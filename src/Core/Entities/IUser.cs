using Core.ActionResults;
using Core.Entities;
using Core.Interfaces;

namespace ApplicationCore.Entities
{
    public interface IUser : IAggregateRoot
    {
        string Id { get; set; }
        string UserName { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }
        public BaseActionResult SetEmail(string email);
        public BaseActionResult SetUserName(string userName);
    }
}