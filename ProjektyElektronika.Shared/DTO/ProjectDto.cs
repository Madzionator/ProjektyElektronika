using System;
using System.Collections.Generic;

namespace ProjektyElektronika.Shared.DTO
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<AuthorDto> Authors { get; set; }
        public string Address { get; set; }
    }
}
