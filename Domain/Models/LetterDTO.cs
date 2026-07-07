namespace Domain.Models
{
    public class LetterDTO : BaseLetterModel
    {
        public UserDTO Addressee { get; set; }
        public Guid AddresseeId { get; set; }
        public UserDTO Recipient { get; set; }
        public Guid RecipientId { get; set; }
        public string AdresseeName { get; set; }
        public bool IsReaden { get; set; }
        public bool Starred { get; set; }
        public DateTime SendTime { get; set; }
    }
}
