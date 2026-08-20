using Domain.Intefraces;
using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Mappers;
using Server.Validators;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DraftController : ControllerBase
    {
        private readonly ValidationCheck _validation;
        private IDraftService _draftService;
        private ILetterService _letterService;
        public DraftController(IDraftService draftService, ValidationCheck validation, ILetterService letterService)
        {
            _letterService = letterService;
            _draftService = draftService;
            _validation = validation;
        }

        [Authorize]
        [HttpGet("{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> Get(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var drafts = await _draftService.GetUserDraftsAsync(userId, startIndex, endIndex);
            return Ok(drafts);
        }

        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] NewDraftDTO request)
        {
            OperationResult result = 
                await _validation.ValidateWriteDraftRequest(request);
            if (!result.Sucsessed)
            {
                return BadRequest(result.ErrorMessage);
            }
            Guid adresseeId = Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (!String.IsNullOrEmpty(request.DraftId))
            {
                await Save(request, Guid.Parse(request.DraftId));
                return Ok(request.DraftId);
            }
            else
            {
                Guid newDraftId = await _draftService.AddDraftAsync(request, adresseeId);
                return Ok(newDraftId.ToString());
            }
        }

        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _draftService.GetTotalAcceptCountAsync(userId);
            return Ok(total);
        }

        [Authorize]
        [HttpGet("{draftId:guid}")]
        public async Task<IActionResult> GetById(Guid draftId)
        {
            DraftDTO draft = await _draftService.GetByIdAsync(draftId);
            return Ok(draft);
        }

        [Authorize]
        [HttpPatch("{draftId:guid}")]
        public async Task<IActionResult> Save([FromBody] NewDraftDTO request, Guid draftId)
        {
            OperationResult result = await _validation.ValidateWriteDraftRequest(request);
            if (!result.Sucsessed)
            {
                return BadRequest(result.ErrorMessage);
            }
            await _draftService.SaveDraftAsync(request, draftId);
            return Ok();
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] NewDraftDTO request)
        {
            NewLetterDTO newLetter = LetterMapper.DraftDTOToLetterDTO(request);
            string[] recipients = request.Recipients.Split(" ");
            OperationResult result = await _validation.ValidateWriteLetterRequest(newLetter, recipients);
            if (!result.Sucsessed)
            {
                return BadRequest(result.ErrorMessage);
            }

            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _letterService.CreateLetterAsync(newLetter, adresseeId, recipients);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{draftId:guid}")]
        public async Task<IActionResult> DeleteDraft(Guid draftId)
        {
            await _draftService.DeleteDraftAsync(draftId);
            return Ok();
        }
    }
}
