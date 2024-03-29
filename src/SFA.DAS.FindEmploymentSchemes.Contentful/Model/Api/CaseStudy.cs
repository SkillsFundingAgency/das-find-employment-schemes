﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api
{
    //todo: name mismatch with model.content (& other models?)
    [DebuggerDisplay("{Name}")]
    [ExcludeFromCodeCoverage]
    public class CaseStudy
    {
        public string? Name { get; set; }
        public string? DisplayTitle { get; set; }
        public Document? Description { get; set; }
    }
}