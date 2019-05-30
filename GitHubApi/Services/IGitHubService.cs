using GitHubApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubApi.Services
{
    public interface IGitHubService
    {
        /// <summary>
        /// Property used to check for any errors after a search request has been made
        /// </summary>
        bool SearchRepositoryError { get; }
        /// <summary>
        /// Property used to pass any error messaging along
        /// </summary>
        string SearchRepositoryErrorMessage { get; }
        /// <summary>
        /// Property containing the repository list retrieved from the API
        /// </summary>
        IEnumerable<Repository> Repositories { get; }
        /// <summary>
        /// Method used to query the API by particular language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<IEnumerable<Repository>> SearchTopLanguagesByStars(TopStargazersRequest request);
        Task<IEnumerable<Repository>> Search(Dictionary<string, string> queryParams);
        /// <summary>
        /// Method used to filter out the result set and return the top 'n' results
        /// </summary>
        /// <param name="cnt"></param>
        /// <returns></returns>
        IEnumerable<Repository> TopStarGazers(int n);
    }
}
