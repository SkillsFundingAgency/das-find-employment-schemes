using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Employer.FrontDoor.Contentful.TestHarness.Model
{
    public interface IFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
