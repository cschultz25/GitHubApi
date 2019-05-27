using GitHubApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private IGitHubService _gitHubClient { get; set; }

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubClient = gitHubService;
        }

        /// <summary>
        /// Method used to search the GitHub API for the top 5 starred repositories
        /// </summary>
        /// <param name="lang">programming language</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<object>>> Get([FromQuery] string lang)
        {
            await _gitHubClient.SearchByLanguage(lang);

            if (!_gitHubClient.SearchRepositoryError)
                return Ok(_gitHubClient.Top(5));
            else
                return BadRequest("Error retrieve response from GitHub api.");
        }

        [HttpGet("ping")]
        public ActionResult<DateTime> Ping()
        {
            return DateTime.UtcNow;
        }
    }       
}
