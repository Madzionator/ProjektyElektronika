namespace ProjektyElektronika.Client.Models
{
    public class Author
    {
        public string Name { get; set; }
        public string Index { get; set; }

        public override string ToString() => $"{Index}:{Name}";
    }
}
