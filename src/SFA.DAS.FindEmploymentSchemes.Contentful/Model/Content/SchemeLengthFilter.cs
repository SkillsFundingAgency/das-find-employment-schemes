//using System.Diagnostics;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

//namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
//{
//    [DebuggerDisplay("{Id}")]
//    public class SchemeLengthFilterAspect : IFilterAspect
//    {
//        public string Id { get; set; }
//        public string Description { get; set; }
//        public bool Selected { get; set; }

//        //todo: yukk - need factory/builder to create immutable filter using generics
//        public SchemeLengthFilterAspect()
//        {
//            Id = null!;
//            Description = null!;
//        }

//        public SchemeLengthFilterAspect(string id, string description, bool selected = false)
//        {
//            Id = id;
//            Description = description;
//            Selected = selected;
//        }
//    }
//}
