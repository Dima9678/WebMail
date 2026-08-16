namespace Domain.Models.DTO
{
    public class LetterDTO : BaseLetterModel
    {
        public UserDTO Addressee { get; set; }
        public Guid AddresseeId { get; set; }
        public List<UserDTO> Recipients { get; set; }

        public string AdresseeName { get; set; }
        public string AdresseeSurname { get; set; }
        public string AdresseeEmail { get; set; }

        public string RecipientName { get; set; }
        public string RecipientSurname { get; set; }
        public string RecipientEmail { get; set; }

        public bool? Forwarded { get; set; }
        public UserDTO? ForwardRecipient { get; set; }
        public Guid? ForwardRecipientId { get; set; }
        public UserDTO? OriginalAuthor { get; set; }


        public DateTime SendTime { get; set; }
        public LetterStateDTO State { get; set; }

        public List<LetterDTO> ChildrenLetters { get; set; } = [];
        public LetterDTO? ParentLetter { get; set; }
        public Guid? ParentLetterId { get; set; }
    }
}
