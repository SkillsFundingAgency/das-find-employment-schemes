using Contentful.Core;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{

    public class SchemeFilterService : ISchemeFilterService
    {

        private readonly ILogger<SchemeFilterService> _logger;

        public SchemeFilterService(ILogger<SchemeFilterService> logger) 
        {
        
            _logger = logger;
        
        }

        /// <summary>
        /// Retrieves scheme filters from the Contentful CMS using the provided Contentful client.
        /// </summary>
        /// <param name="contentfulClient">The Contentful client used to retrieve data from the CMS.</param>
        /// <returns>Returns a list of scheme filters if available, otherwise returns an empty list.</returns>
        public async Task<List<SchemeFilter>> GetSchemeFilters(IContentfulClient contentfulClient)
        {

            _logger.LogInformation("Beginning {MethodName}...", nameof(GetSchemeFilters));

            try
            {

                var builder = QueryBuilder<SchemeFilter>.New.ContentTypeIs("schemeFilter");

                var filters = await contentfulClient.GetEntries(builder);

                if (filters.Any())
                {

                    _logger.LogInformation("Retrieved scheme filters: {SchemeFilterCount}", filters.Count());

                    return filters.OrderBy(t => t.SchemeFilterOrder).ToList();

                }
                else
                {

                    _logger.LogInformation("No scheme filters found.");

                    return new List<SchemeFilter>();

                }

            }
            catch (Exception _Exception)
            {

                _logger.LogError(_Exception, "Unable to get scheme filters.");

                return new List<SchemeFilter>();

            }

        }

    }

}
