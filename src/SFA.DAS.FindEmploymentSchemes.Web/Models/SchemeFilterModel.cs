using System;
using System.Linq;
using System.Text;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class SchemeFilterModel
    {

        public string[] FilterAspects { get; set; }

        public SchemeFilterModel()
        {

            FilterAspects = Array.Empty<string>();

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

                if (FilterAspects.Any())
                {

                    queryBuilder.Append(string.IsNullOrWhiteSpace(queryBuilder.ToString()) ? "filters=" : "&filters=");

                    queryBuilder.Append(Uri.EscapeDataString(string.Join(',', FilterAspects)));

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
