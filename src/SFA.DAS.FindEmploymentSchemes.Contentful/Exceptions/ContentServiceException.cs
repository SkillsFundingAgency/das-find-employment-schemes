using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ContentServiceException : Exception
    {
        public ContentServiceException()
        {
        }

        protected ContentServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ContentServiceException(string? message) : base(message)
        {
        }

        public ContentServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
