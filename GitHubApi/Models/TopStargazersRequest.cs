using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GitHubApi.Models
{
    public class TopStargazersRequest
    {
        [FromQuery(Name = "lang")]      
        [Required]
        public string Language { get; set; }
    }
}
