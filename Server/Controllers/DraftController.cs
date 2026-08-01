using Domain.Models.DTO;
using Domain.Models.Requests;
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
        [HttpGet("{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> Get(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var drafts = await _draftService.GetUserDrafts(userId, startIndex, endIndex);
            return Ok(drafts);
        }

        [Authorize]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] NewDraftDTO request)
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
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _draftService.GetTotalAcceptCount(userId);
            return Ok(total);
        }

        [Authorize]
        [HttpGet("{draftId:guid}")]
        public async Task<IActionResult> GetById(Guid draftId)
        {
            DraftDTO draft = await _draftService.GetById(draftId);
            return Ok(draft);
        }

        [Authorize]
        [HttpPatch("{draftId:guid}")]
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
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] NewDraftDTO request)
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
