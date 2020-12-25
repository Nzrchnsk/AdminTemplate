using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.ActionResults
{
    public class BaseActionResult
    {
        private BaseActionResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">Core.Constants.ActionStatuses</param>
        /// <param name="message"></param>
        /// <exception cref="ArgumentException"></exception>
        public BaseActionResult(int status, string message)
        {
            if (!((IEnumerable<int>) Enum.GetValues(typeof(Constants.ActionStatuses))).Contains(status))
            {
                throw new ArgumentException();
            }

            Enum.GetUnderlyingType(typeof(Constants.ActionStatuses));

            Status = status;
            Message = message;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="status">Core.Constants.ActionStatuses</param>
        /// <exception cref="ArgumentException"></exception>
        public BaseActionResult(int status)
        {
            if (!((IEnumerable<int>) Enum.GetValues(typeof(Constants.ActionStatuses))).Contains(status))
            {
                throw new ArgumentException();
            }

            Status = status;
            Message = "";
        }

        public int Status { get; private set; }

        public string Message { get; private set; }
        
        
    }
}