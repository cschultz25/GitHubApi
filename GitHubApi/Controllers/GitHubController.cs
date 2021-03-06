using GitHubApi.Models;
using GitHubApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private IGitHubService _gitHubClient { get; }
        private ILogger _logger { get; }

        public GitHubController(IGitHubService gitHubService, ILogger<GitHubController> logger)
        {
            _gitHubClient = gitHubService;
            _logger = logger;
        }

        /// <summary>
        /// Method used to search the GitHub API for the top 5 starred repositories
        /// </summary>
        /// <param name="lang">URL encoded language value</param>
        /// <returns></returns>                
        [HttpGet("Repositories/Filter/StarGazers")]
        public async Task<ActionResult<IEnumerable<object>>> GetTopStarGazers([FromQuery] TopStargazersRequest request)
        {
            try
            {
                await _gitHubClient.SearchTopLanguagesByStars(request);
                
                if (!_gitHubClient.SearchRepositoryError)
                    return Ok(_gitHubClient.TopStarGazers(request.ResultCount));
                else
                    return BadRequest($"Error retrieve response from GitHub api. {_gitHubClient.SearchRepositoryErrorMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception occurred processing GetTopStarGazers request.");
                return StatusCode(500, $"Uh-oh.  Something unknown went wrong processing the request");
            }
        }

        [HttpGet("ping")]
        public ActionResult<DateTime> Ping()
        {
            var ping = DateTime.UtcNow;
            _logger.LogInformation($"Ping: {ping}");
            return ping;
        }
    }
}
