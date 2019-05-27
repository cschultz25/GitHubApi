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
        /// Property containing the repository list retrieved from the API
        /// </summary>
        IOrderedEnumerable<object> Repositories { get; }
        /// <summary>
        /// Method used to query the API by particular language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<IEnumerable<object>> SearchByLanguage(string language);
        /// <summary>
        /// Method used to filter out the result set and return the top 'n' results
        /// </summary>
        /// <param name="cnt"></param>
        /// <returns></returns>
        IEnumerable<object> Top(int n);
    }
}
