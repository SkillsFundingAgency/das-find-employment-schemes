using System;
using System.Runtime.Serialization;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions
{
    [Serializable]
    public class ConfigurationMissingException : Exception
    {
        public ConfigurationMissingException()
        {
        }

        protected ConfigurationMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConfigurationMissingException(string? message) : base(message)
        {
        }

        public ConfigurationMissingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
