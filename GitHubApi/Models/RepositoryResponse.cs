using System.Collections.Generic;

namespace GitHubApi.Models
{
    public class RepositoryResponse
    {
        public int Total_Count { get; set; }
        public bool Incomplete_Results { get; set; }
        public IEnumerable<Repository> Items { get; set; }
    }
}
