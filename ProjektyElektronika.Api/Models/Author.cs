using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektyElektronika.Api.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index => Id;

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
