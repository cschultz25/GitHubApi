﻿using GitHubApi.Configuration;
using GitHubApi.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public IEnumerable<Repository> Repositories { get; private set; }
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
        public IEnumerable<Repository> TopStarGazers(int count)
        {
            var repoCnt = Repositories.Count();
            if (repoCnt < count)
                return Repositories.OrderByDescending(x=>x.StarGazers_Count).Take(repoCnt);
            else
                return Repositories.OrderByDescending(x=>x.StarGazers_Count).Take(count);
        }

        /// <summary>
        /// Will query the GitHub API for all repositories within a given language and order the result set
        /// in a descending fashion by the number of stars
        /// </summary>
        /// <param name="language">Search parameter for the API</param>
        /// <returns>Repsitory objects</returns>
        public async Task<IEnumerable<Repository>> SearchTopLanguagesByStars(string language)
        {            
            var searchQry = BuildQuery(language: language,
                                       sort: "stars",
                                       order: "desc");

            return await Search(searchQry);
        }

        public async Task<IEnumerable<Repository>> Search(Dictionary<string, string> queryParams)
        {
            SearchRepositoryError = false;
            SearchRepositoryErrorMessage = string.Empty;
            try
            {
                var searchUri = QueryHelpers.AddQueryString(GitHubConstants.SearchRepositories, queryParams);

                await ProcessResponse(await _httpClient.GetAsync(searchUri));
            }
            catch (Exception ex)
            {
                SearchRepositoryError = true;
                SearchRepositoryErrorMessage = "Unhandled exception occurred while processing Github search request";
                Repositories = default;
                _logger.LogError(ex, SearchRepositoryErrorMessage);
            }

            return Repositories;
        }

        private async Task ProcessResponse(HttpResponseMessage response)
        {           
            if (response.IsSuccessStatusCode)
            {                
                var responseObj = await response.Content.ReadAsAsync<RepositoryResponse>();
                Repositories = responseObj.Items;
            }
            else
            {
                //ToDo: interpret different api responses to better support integration
                switch (response.StatusCode)
                {
                    case HttpStatusCode.UnprocessableEntity:
                        SearchRepositoryErrorMessage = "Inavlid language submitted.  Github is unable to process the request";
                        break;
                    default:
                        SearchRepositoryErrorMessage = "Non success status code returned from the github Api";
                        break;
                }
                SearchRepositoryError = true;
                Repositories = default;
                _logger.LogError(SearchRepositoryErrorMessage);
            }
        }

        private Dictionary<string, string> BuildQuery(string language = null, string sort = null, string order = null)
        {
            var qry = new Dictionary<string, string>();

            if (language != null)
                qry.Add("q", $"language:{System.Web.HttpUtility.UrlEncode(language)}");

            if (sort != null)
                qry.Add("sort", sort);

            if (order != null)
                qry.Add("order", order);

            return qry;
        }
    }
}
