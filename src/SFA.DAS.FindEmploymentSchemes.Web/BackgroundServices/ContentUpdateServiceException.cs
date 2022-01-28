using System.Runtime.Serialization;
using System;

namespace SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices
{
    // if we need more than 1 exception, we could have one exception for the web
    [Serializable]
    public class ContentUpdateServiceException : Exception
    {
        public ContentUpdateServiceException()
        {
        }

        protected ContentUpdateServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ContentUpdateServiceException(string? message) : base(message)
        {
        }

        public ContentUpdateServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
