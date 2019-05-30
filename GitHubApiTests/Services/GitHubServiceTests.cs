using GitHubApi.Configuration;
using GitHubApi.Models;
using GitHubApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        [TestCase("js", "Javascript")]
        [TestCase("c", "C")]
        [TestCase("C#", "C#")]
        [TestCase("c++", "C++")]
        [TestCase("Assembly", "Assembly")]
        //Can only run this test twice in a 1 minute window due to github throttling
        public async Task SimpleGitHubServiceValidation(string lang, string expectedLang)
        {
            var expectedResult = 5;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GitHubConstants.GitHubDomain);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //User Agent header is a required header in the request to their api
                client.DefaultRequestHeaders.UserAgent.TryParseAdd("GitHubApi-SampleRequest-Test");

                _gitHubService = new GitHubService(client, Mock.Of<ILogger<GitHubService>>());

                //Receiving a runtime dependency issue when processing this request
                var result = await _gitHubService.SearchTopLanguagesByStars(new TopStargazersRequest { Language = lang, OrderBy = OrderBy.Desc, ResultCount = expectedResult });

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IEnumerable<Repository>>(result);
                Assert.AreEqual(5, _gitHubService.TopStarGazers(expectedResult).Count());
                Assert.IsNotNull(_gitHubService.Repositories.FirstOrDefault(x => x.Language.ToLower() == expectedLang.ToLower()));
            }
        }
    }
}