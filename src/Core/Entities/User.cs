using Core.Entities;
using Core.Interfaces;

namespace ApplicationCore.Entities
{
    public class User : BaseEntity, IAggregateRoot
    {
        private User()
        {
        }

        public User(string userName, string passwordHash)
        {
        }

        int Id { get; set; }
        string UserName { get; set; }
        string PasswordHash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns>1 - success</returns>
        public int SetPassword(string password)
        {
            return 0;
        }
    }
}