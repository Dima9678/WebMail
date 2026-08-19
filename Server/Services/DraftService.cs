using Domain;
using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Persistence;
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

        public async Task<DraftDTO> GetByIdAsync(Guid draftId)
        {
            Draft draft = await _db.Drafts
                .Include(x => x.Author)
                .SingleOrDefaultAsync(x => x.Id == draftId);
            DraftDTO draftDTO = DraftMapper.ToDTO(draft);
            return draftDTO;
        }
        public async Task<List<DraftDTO>> GetUserDraftsAsync(Guid UserId, int startIndex, int endIndex)
        {
            List<DraftDTO> draftsList = await _db.Drafts
                .Where(l => l.AuthorId == UserId)
                .Include(l => l.Author)
                .OrderByDescending(l => l.LastEditDate)
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
        public async Task<int> GetTotalAcceptCountAsync(Guid userId)
        {
            int count = await _db.Drafts
                .Where(l => l.AuthorId == userId)
                .CountAsync();

            return count;
        }

        public async Task<Guid> AddDraftAsync(NewDraftDTO request, Guid authorId)
        {
            User? user = await _db.Users.SingleOrDefaultAsync(x => x.Id == authorId);
            
            Draft draft = new Draft()
            {

                Author = user,
                AuthorId = user.Id,
                Recipients = request.Recipients,
                Title = request.Title,
                Text = request.Text,
                LastEditDate = DateTime.UtcNow,
            };

            _db.Drafts.Add(draft);
            _db.SaveChanges();

            return draft.Id;
        }
        public async Task DeleteDraftAsync(Guid draftId)
        {
            _db.Drafts.Remove(new Draft {Id = draftId});
            await _db.SaveChangesAsync();
        }
        public async Task SaveDraftAsync(NewDraftDTO request, Guid draftId)
        {
            Draft? draftInDb = await _db.Drafts.SingleOrDefaultAsync(x => x.Id == draftId);

            draftInDb?.Recipients = request.Recipients;
            draftInDb?.Title = request.Title;
            draftInDb?.Text = request.Text;

            draftInDb?.LastEditDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
