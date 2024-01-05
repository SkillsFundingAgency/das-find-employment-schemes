using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions
{
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class ConfigurationMissingException : Exception
    {
        public ConfigurationMissingException()
        {
        }

        [Obsolete("The base constructor is marked obsolete", DiagnosticId = "SYSLIB0051")]
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
