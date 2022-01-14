//using System;
//using System.Collections.Generic;
//using System.Text;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

//namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
//{
//    public class Filter : IFilter
//    {
//        public string Id { get; private set; }
//        public string Description { get; private set; }
//        public bool Selected { get; private set; }

//        private Filter()
//        {
//            Id = null!;
//            Description = null!;
//            Selected = false;
//        }

//        public static T Create<T>(string id, string description, bool selected = false)
//            where T : IFilter, new()
//        {
//            return new T
//            {
//                Id = id,
//                Description = description,
//                Selected = selected
//            };
//        }
//    }
//}
