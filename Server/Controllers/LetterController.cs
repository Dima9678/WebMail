using Domain.Intefraces;
using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Normaizators;
using Server.Validators;
using System.Security.Claims;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LetterController : ControllerBase
    {
        private readonly ValidationCheck _validation;

        private ILetterService _letterService;

        public LetterController(ILetterService letterService, ValidationCheck validation)
        {
            _letterService = letterService;
            _validation = validation;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewLetterDTO request)
        {
            string[] recipients = InputNormalizator.SplitEmails(request.Recipients);
            
            OperationResult result = await _validation.ValidateWriteLetterRequest(request, recipients);
            
            if (!result.Sucsessed)
            {
                return BadRequest(result.ErrorMessage);
            }
            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _letterService.CreateLetterAsync(request, adresseeId, recipients);
            return Ok();
        }

        [Authorize]
        [HttpGet("{letterId:guid}")]
        public async Task<IActionResult> GetById(Guid letterId, [FromQuery] string from)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            FullLetterDTO fullLetterDto = await _letterService.GetByIdAsync(letterId, userId, from);

            if (fullLetterDto == null)
            {
                return BadRequest("Письмо не найдено");
            }
            else
            {
                return Ok(fullLetterDto);
            }
        }

        [Authorize]
        [HttpGet("inbox/{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> GetInbox(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetAcceptLettersAsync(userId, startIndex, endIndex);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpGet("inbox/count")]
        public async Task<IActionResult> GetInboxCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _letterService.GetAcceptCountAsync(userId);
            return Ok(total);
        }

        [Authorize]
        [HttpGet("sent/{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> GetSent(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetSentLettersAsync(userId, startIndex, endIndex);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpGet("sent/count")]
        public async Task<IActionResult> GetSentCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _letterService.GetSendCountAsync(userId);
            return Ok(total);
        }

        [Authorize]
        [HttpGet("spam/{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> GetSpam(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetSpamLettersAsync(userId, startIndex, endIndex);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpGet("spam/count")]
        public async Task<IActionResult> GetSpamCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
           int count = await _letterService.GetSpamCountAsync(userId);
           return Ok(count);
        }

        [Authorize]
        [HttpGet("starred/{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> GetStarred(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetStarredLettersAsync(userId, startIndex, endIndex);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpGet("starred/count")]
        public async Task<IActionResult> GetStarredCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int count = await _letterService.GetStarredCountAsync(userId);
            return Ok(count);
        }

        [Authorize]
        [HttpPatch("{letterid:guid}/toggle-starred")]
        public async Task<IActionResult> ToggleStarred(Guid letterid)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _letterService.ChangeStarredAsync(letterid, userId);
            return Ok();
        }

        [Authorize]
        [HttpPatch("{letterid:guid}/toggle-read")]
        public async Task<IActionResult> ToggleRead(Guid letterid)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _letterService.ChangeIsReadenAsync(letterid, userId);
            return Ok();
        }

        [Authorize]
        [HttpPatch("{letterid:guid}/toggle-spam")]
        public async Task<IActionResult> ToggleSpam(Guid letterid)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _letterService.ChangeIsSpamAsync(letterid, userId);
            return Ok();
        }

        [Authorize]
        [HttpPost("forward")]
        public async Task<IActionResult> Forward([FromBody]ForwardRequest request)
        {
            OperationResult validationResult = _validation.CorrectEmail(request.ForwardEmail);
            if (!validationResult.Sucsessed)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            OperationResult forwardResult = await _letterService.ForwardAsync(request, userId);
            if (forwardResult.Sucsessed)
            {
                return Ok();
            }
            else
            {
                return BadRequest(forwardResult.ErrorMessage);
            }
        }

        [Authorize]
        [HttpPost("reply/{parentLetterId:guid}")]
        public async Task<IActionResult> Reply([FromBody] ReplyDTO reply, Guid parentLetterId)
        {
            OperationResult result = await _validation.ValidateReplyRequest(reply);
            if (!result.Sucsessed)
            {
                return BadRequest(result.ErrorMessage);
            }

            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _letterService.CreateReplyAsync(reply, adresseeId, parentLetterId);
            return Ok();
        }
    }
}

