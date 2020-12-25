using JetBrains.Annotations;

namespace SharedDto
{
    public class ResponseModel<T> where T : class
    {
        public ResponseModel ( string message, [CanBeNull] T content)
        {
            Message = message;
            Content = content;
        }
        public string Message { get; private set; }
        [CanBeNull] public T Content { get; private set; }
    }
}