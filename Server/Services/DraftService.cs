using Domain.Models;
using Domain;
using Domain.Models.DTO;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Server.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace Server.Service
{
    public class DraftService
    {
        private readonly DatabaseContext _db;
        public DraftService(DatabaseContext db)
        {
            _db = db;
        }


        public async Task<List<DraftDTO>> GetUserDrafts(Guid  UserId, int startIndex, int endIndex)
        {
            List<DraftDTO> draftsList = await _db.Drafts
                .Where(l => l.AuthorId == UserId)
                .Include(l => l.Author)
                .Select(l => DraftMapper.ToDTO(l)).ToListAsync();

            List<DraftDTO> drafts = new List<DraftDTO>();

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i < draftsList.Count)
                {
                    drafts.Add(draftsList[i]);
                }
                else
                {
                    break;
                }
            }

            return drafts;
        }

        public async Task Add(NewDraftDTO request, Guid authorId)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(x => x.Id == authorId);
            
            Draft draft = new Draft()
            {

                Author = user,
                AuthorId = user.Id,
                RecipientEmail = request.RecipientEmail,
                Title = request.Title,
                Text = request.Text,
                LastEditDate = DateTime.UtcNow,
            };

            _db.Drafts.Add(draft);
            _db.SaveChanges();
        }
        public async Task<DraftDTO> GetById(Guid draftId)
        {
            Draft draft = await _db.Drafts
                .Include(x => x.Author)
                .SingleOrDefaultAsync(x => x.Id == draftId);
            DraftDTO draftDTO = DraftMapper.ToDTO(draft);
            return draftDTO;
        }
        public async Task Save(NewDraftDTO request, Guid draftId)
        {
            Draft? draftInDb = await _db.Drafts.SingleOrDefaultAsync(x => x.Id == draftId);

            draftInDb?.RecipientEmail = request.RecipientEmail;
            draftInDb?.Title = request.Title;
            draftInDb?.Text = request.Text;

            draftInDb?.LastEditDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
        public async Task<int> GetTotalAcceptCount(Guid userId)
        {
            int count = await _db.Drafts
                .Where(l => l.AuthorId == userId)
                .CountAsync();

            return count;
        }
    }
}
