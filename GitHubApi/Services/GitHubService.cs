using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubApi.Services
{
    /// <summary>
    /// Service class to integrate with the GitHub API
    /// </summary>
    public class GitHubService : IGitHubService
    {
        private HttpClient _httpClient { get; }
        private ILogger _logger { get; }

        public IOrderedEnumerable<object> Repositories { get; private set; }
        public bool SearchRepositoryError { get; private set; }
        public string SearchRepositoryErrorMessage { get; private set; }

        public GitHubService(HttpClient httpClient, ILogger<GitHubService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Method to return Top n values from the result set.  Will default to current size of the Repository if 
        /// the value provided is larger than the data set
        /// </summary>
        /// <param name="count"></param>
        /// <returns>Repository objects</returns>
        public IEnumerable<object> Top(int count)
        {
            var repoCnt = Repositories.Count();
            if (repoCnt < count)
                return Repositories.Take(repoCnt);
            else
                return Repositories.Take(count);
        }

        /// <summary>
        /// Will query the GitHub API for all repositories within a given language and order the result set
        /// in a descending fashion by the number of stars
        /// </summary>
        /// <param name="language">Search parameter for the API</param>
        /// <returns>Repsitory objects</returns>
        public async Task<IEnumerable<object>> SearchByLanguage(string language)
        {
            SearchRepositoryError = false;

            try
            {
                //returned elements should be in descending order by star count
                var response = await _httpClient.GetAsync("/search/repositories?q=language:c&sort=stars&order=desc");

                //ToDo: add some logic to inspect result and set some public variables
                if (response.IsSuccessStatusCode)
                {
                    Repositories = await response.Content
                        .ReadAsAsync<IOrderedEnumerable<object>>();
                }
                else
                {
                    SearchRepositoryError = true;
                    SearchRepositoryErrorMessage = "Non success status code returned from the github Api";
                    Repositories = default;
                    _logger.LogError(SearchRepositoryErrorMessage);
                }
            }catch(Exception ex)
            {
                SearchRepositoryError = true;
                SearchRepositoryErrorMessage = "Unhandled exception occurred while processing Github search request";
                Repositories = default;
                _logger.LogError(ex, SearchRepositoryErrorMessage);
            }
        
            return Repositories;
        }
    }
}
