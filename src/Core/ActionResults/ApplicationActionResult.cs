namespace Core.ActionResults
{
    public class ApplicationActionResult<TResult> : BaseActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">Core.Constants.ActionStatuses</param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentException"></exception>

        public ApplicationActionResult(int status, string message) : base(status, message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">Core.Constants.ActionStatuses</param>
        /// <exception cref="ArgumentException"></exception>

        public ApplicationActionResult(int status) : base(status)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="status">Core.Constants.ActionStatuses</param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentException"></exception>
        public ApplicationActionResult(TResult result, int status, string message):base(status, message)
        {
            Result = result;
        }

        public TResult Result { get; private set; }
    }
}