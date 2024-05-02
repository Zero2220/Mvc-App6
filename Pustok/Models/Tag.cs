namespace Pustok.Models
{
    public class Tag:BaseEntity
    {
        public string Name { get; set; }

        public List<BookTags> BookTags { get; set; } 
    }
}
