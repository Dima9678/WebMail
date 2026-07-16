using Domain.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Server.Service;
using Server.Validators;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LetterController : ControllerBase
    {
        private readonly ValidationCheck _validation;

        private LetterService _letterService;
        
        public LetterController(LetterService letterService, ValidationCheck validation)
        {
            _letterService = letterService;
            _validation = validation;
        }

        [Authorize]
        [HttpPost("write")]
        public async Task<IActionResult> Write([FromBody] NewLetterDTO request)
        {
            bool result;
            string message;
            (result, message) = await _validation.ValidateWriteLetterRequest(request);
            if (!result) 
            {
                return BadRequest(message);
            }
            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _letterService.Add(request, adresseeId);
            return Ok();
        }

        [HttpGet("{letterId:guid}")]
        public async Task<IActionResult> GetById(Guid letterId)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            LetterDTO letterDTO = await _letterService.GetById(letterId, userId);

            return Ok(letterDTO);
        }

        [Authorize]
        [HttpGet("getuserletters")]
        public async Task<IActionResult> GetUserAcceptLetters()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetAcceptLetters(userId);
            return Ok(userLetters);
        }
        [Authorize]
        [HttpGet("getusersentletters")]
        public async Task<IActionResult> GetUserSentLetters()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetSentLetters(userId);
            return Ok(userLetters);
        }
        [Authorize]
        [HttpGet("getuserstarredletters")]
        public async Task<IActionResult> GetUserStarredLetters()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetStarredLetters(userId);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpPut("changestarred/{letterid:guid}")]
        public async Task<IActionResult> ChangeStarred(Guid letterid)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _letterService.ChangeStarred(letterid, userId);
            return Ok();
        }

    }
}

    