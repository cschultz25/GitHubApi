using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GitHubApi.Models
{
    public class TopStargazersRequest
    {
        [FromQuery(Name = "lang")]      
        [Required]
        public string Language { get; set; }
        [FromQuery(Name = "order")]
        public OrderBy OrderBy { get; set; } = OrderBy.Desc;
        [FromQuery(Name = "cnt")]
        [Range(1, 25)]
        public int ResultCount { get; set; } = 25;
    }

    public enum OrderBy
    {
        Desc,
        Asc
    }
}
