using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SFA.DAS.FindEmploymentSchemes.Web.BackgroundServices
{
    // if we need more than 1 exception, we could have one exception for the web
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ContentUpdateServiceException : Exception
    {
        public ContentUpdateServiceException()
        {
        }

        [Obsolete("The base constructor is marked obsolete", DiagnosticId = "SYSLIB0051")]
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
