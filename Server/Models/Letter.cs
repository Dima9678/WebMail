namespace Server.Models
{
    public class Letter
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Guid AddresseeId { get; set; }
        public User Addressee { get; set; }
        public Guid RecipientId { get; set; }
        public User Recipient { get; set; }
        public bool IsSent { get; set; }

        public Letter()
        {
            Id = Guid.NewGuid();
            AddresseeId = Guid.NewGuid();
            RecipientId = Guid.NewGuid();
        }
    }
}
