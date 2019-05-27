using System;
using Newtonsoft.Json;

namespace GitHubApi.Models
{
    public class Repository
    {
        public int Id { get; set; }        
        public string Node_Id { get; set; }
        public string Name { get; set; }        
        public string Full_Name { get; set; }
        public bool Private { get; set; }
        public object Owner { get; set; }        
        public Uri Html_Url { get; set; }
        public string Description { get; set; }
        public bool Fork { get; set; }
        public Uri Url { get; set; }

        //ToDo: Fill out the rest of the object if needed

        public int StarGazers_Count { get; set; }
        public string Language { get; set; }
    }
}
