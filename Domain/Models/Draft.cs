namespace Domain.Models
{
    public class Draft : BaseLetterModel
    {
        public string RecipientEmail { get; set; }
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime LastEditDate { get; set; }
        public Draft()
        {
            Id = Guid.NewGuid();
        }
    }
}
