using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class SchemeFilterModel
    {

        public string[] Motivations { get; set; }

        public string[] SchemeLength { get; set; }

        public string[] Pay { get; set; }

        public IEnumerable<string> AllFilters => Motivations.Union(SchemeLength).Union(Pay);

        public SchemeFilterModel()
        {

            Motivations = Array.Empty<string>();

            SchemeLength = Array.Empty<string>();

            Pay = Array.Empty<string>();

        }

        /// <summary>
        /// Use the current filters to build out a prefixed filter URL for page sharing.
        /// </summary>
        /// <returns>System.String URL in the format of ?pay=&duration=&motivation=</returns>
        public string BuildFilterQueryString()
        {

            try
            {

                StringBuilder queryBuilder = new StringBuilder();

                if (Pay.Any())
                {

                    queryBuilder.Append(string.IsNullOrWhiteSpace(queryBuilder.ToString()) ? "pay=" : "&pay=");

                    queryBuilder.Append(Uri.EscapeDataString(string.Join(',', Pay)));

                }

                if (SchemeLength.Any())
                {

                    queryBuilder.Append(string.IsNullOrWhiteSpace(queryBuilder.ToString()) ? "duration=" : "&duration=");

                    queryBuilder.Append(Uri.EscapeDataString(string.Join(',', SchemeLength)));

                }

                if (Motivations.Any())
                {

                    queryBuilder.Append(string.IsNullOrWhiteSpace(queryBuilder.ToString()) ? "motivation=" : "&motivation=");

                    queryBuilder.Append(Uri.EscapeDataString(string.Join(',', Motivations)));

                }

                return queryBuilder.ToString();

            }
            catch
            {

                return string.Empty;

            }

        }

    }

}
