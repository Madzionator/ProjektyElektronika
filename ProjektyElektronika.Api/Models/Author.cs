namespace ProjektyElektronika.Api.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Index { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
