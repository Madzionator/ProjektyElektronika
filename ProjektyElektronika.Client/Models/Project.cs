using MvvmHelpers;
using System;
using Newtonsoft.Json;

namespace ProjektyElektronika.Client.Models
{

    public class Project : ObservableObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
       
        [JsonProperty("academic_year")]
        public int AcademicYear { get; set; } // delet this
        
        [JsonProperty("is_diploma")]
        public bool IsDiploma { get; set; }

        [JsonProperty("files_link")]
        public string FilesLink { get; set; }

        [JsonProperty("internal_filename")]
        public string DownloadName { get; set; }

        public bool IsDownloaded { get; set; }
        public string LocalAddress { get; set; }
    }
}
