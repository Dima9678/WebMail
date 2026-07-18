using Domain.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Mappers;
using Server.Service;
using Server.Validators;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DraftController : ControllerBase
    {
        private readonly ValidationCheck _validation;
        private DraftService _draftService;
        private LetterService _letterService;
        public DraftController(DraftService draftService, ValidationCheck validation, LetterService letterService)
        {
            _letterService = letterService;
            _draftService = draftService;
            _validation = validation;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetDrafts()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var drafts = await _draftService.Get(userId);
            return Ok(drafts);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddDrafts([FromBody] NewDraftDTO request)
        {
            bool result;
            string message;
            (result, message) = await _validation.ValidateWriteDraftRequest(request);
            if (!result)
            {
                return BadRequest(message);
            }
            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _draftService.Add(request, adresseeId);
            return Ok();
        }

        [Authorize]
        [HttpGet("getbyid/{draftId:guid}")]
        public async Task<IActionResult> GetDraftById(Guid draftId)
        {
            DraftDTO draft = await _draftService.GetById(draftId);
            return Ok(draft);
        }

        [Authorize]
        [HttpPatch("save/{draftId:guid}")]
        public async Task<IActionResult> Save([FromBody] NewDraftDTO request, Guid draftId)
        {
            bool result;
            string message;
            (result, message) = await _validation.ValidateWriteDraftRequest(request);
            if (!result)
            {
                return BadRequest(message);
            }
            await _draftService.Save(request, draftId);
            return Ok();
        }

        [Authorize]
        [HttpPost("send/{draftId:guid}")]
        public async Task<IActionResult> Send([FromBody] NewDraftDTO request, Guid draftId)
        {
            bool result;
            string message;

            NewLetterDTO newLetter = LetterMapper.DraftDTOToLetterDTO(request);

            (result, message) = await _validation.ValidateWriteLetterRequest(newLetter);
            if (!result)
            {
                return BadRequest(message);
            }

            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _letterService.Add(newLetter, adresseeId);
            return Ok();
        }

        [Authorize]
        [HttpPost("sendnew")]
        public async Task<IActionResult> SendNew([FromBody] NewDraftDTO request)
        {
            bool result;
            string message;

            NewLetterDTO newLetter = LetterMapper.DraftDTOToLetterDTO(request);

            (result, message) = await _validation.ValidateWriteLetterRequest(newLetter);
            if (!result)
            {
                return BadRequest(message);
            }

            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _letterService.Add(newLetter, adresseeId);
            return Ok();
        }
    }
}
