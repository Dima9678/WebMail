namespace Domain.Models.DTO
{
    public class LetterDTO : BaseLetterModel
    {
        public UserDTO Addressee { get; set; }
        public Guid AddresseeId { get; set; }
        public UserDTO Recipient { get; set; }
        public Guid RecipientId { get; set; }

        public string AdresseeName { get; set; }
        public string AdresseeSurname { get; set; }
        public string AdresseeEmail { get; set; }

        public string RecipientName { get; set; }
        public string RecipientSurname { get; set; }
        public string RecipientEmail { get; set; }

        public DateTime SendTime { get; set; }
        public List<LetterStateDTO> LetterStates { get; set; }

        public List<LetterDTO> ChidrenLetters { get; set; } = [];
        public LetterDTO? ParentLetter { get; set; }
        public Guid? ParentLetterId { get; set; }
    }
}
