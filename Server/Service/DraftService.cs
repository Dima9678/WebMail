using Domain.Models;
using Domain;
using Domain.Models.DTO;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Server.Mappers;

namespace Server.Service
{
    public class DraftService
    {
        private readonly DatabaseContext _db;
        public DraftService(DatabaseContext db)
        {
            _db = db;
        }


        public async Task<List<DraftDTO>> Get(Guid  UserId)
        {
            List<DraftDTO> draftsList = await _db.Drafts
                .Where(l => l.AuthorId == UserId)
                .Include(l => l.Author)
                .Select(l => DraftMapper.ToDTO(l)).ToListAsync();

            return draftsList;
        }
        public async Task Add(NewDraftDTO request, Guid authorId)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(x => x.Id == authorId);
            
            Draft draft = new Draft()
            {
                Author = user,
                AuthorId = user.Id,
                Title = request.Title,
                Text = request.Text,
                LastEditDate = DateTime.UtcNow,
            };

            _db.Drafts.Add(draft);
            _db.SaveChanges();
        }
        public void GetById()
        {

        }
    }
}
