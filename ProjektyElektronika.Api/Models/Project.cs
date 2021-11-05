using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektyElektronika.Api.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<Author> Authors { get; set; }
        public string Address { get; set; }
    }
}
