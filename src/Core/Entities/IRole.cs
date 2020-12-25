using Core.ActionResults;
using Core.Interfaces;

namespace Core.Entities
{
    public interface IRole :IAggregateRoot
    {
        string Name { get; set; }

        public BaseActionResult SetName(string name);
    }
}