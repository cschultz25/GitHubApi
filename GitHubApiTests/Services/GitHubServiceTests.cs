using GitHubApi.Configuration;
using GitHubApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tests
{
    public class GitHubServiceTests
    {
        private GitHubService _gitHubService { get; set; }
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("js")]
        [TestCase("c")]
        public async Task SimpleGitHubServiceValidation(string lang)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GitHubConstants.GitHubDomain);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //User Agent header is a required header in the request to their api
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("GitHubApi-SampleRequest-Test");

                _gitHubService = new GitHubService(client, Mock.Of<ILogger<GitHubService>>());

                var result = await _gitHubService.SearchTopLanguagesByStars(lang);

                Assert.AreEqual(5, result.Count());
            }
        }
    }
}