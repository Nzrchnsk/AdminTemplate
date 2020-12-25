using Core.Interfaces;

namespace Core.Entities
{
    public class Role : BaseEntity, IAggregateRoot
    {
        private Role()
        {
            
        }

        public Role(string name)
        {
            Name = name;
        }
        
        public string Name { get; private set; }
    }
}