using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewLetterDTO request)
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

        [Authorize]
        [HttpGet("{letterId:guid}")]
        public async Task<IActionResult> GetById(Guid letterId)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            FullLetterDTO fullLetterDto = await _letterService.GetById(letterId, userId);

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
            List<LetterDTO> userLetters = await _letterService.GetAcceptLetters(userId, startIndex, endIndex);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpGet("inbox/count")]
        public async Task<IActionResult> GetInboxCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _letterService.GetTotalAcceptCount(userId);
            return Ok(total);
        }

        [Authorize]
        [HttpGet("sent/{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> GetSent(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetSentLetters(userId, startIndex, endIndex);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpGet("sent/count")]
        public async Task<IActionResult> GetSentCount()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _letterService.GetTotalSendCount(userId);
            return Ok(total);
        }

        [Authorize]
        [HttpGet("starred")]
        public async Task<IActionResult> GetInboxStarred()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetStarredLetters(userId);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpPut("{letterid:guid}/toggle-starred")]
        public async Task<IActionResult> ToggleStarred(Guid letterid)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _letterService.ChangeStarred(letterid, userId);
            return Ok();
        }

        [Authorize]
        [HttpPut("{letterid:guid}/toggle-read")]
        public async Task<IActionResult> ToggleRead(Guid letterid)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _letterService.ChangeIsReaden(letterid, userId);
            return Ok();
        }

        [Authorize]
        [HttpPost("forward")]
        public async Task<IActionResult> Forward([FromBody]ForwardRequest request)
        {
            bool valResult = _validation.CorrectEmai(request.ForwardEmail);
            if (!valResult)
            {
                //Контроллер не должен сам писать BadRequest. Должен принимать OperationResult
                return BadRequest("Невалидный Email");
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            OperationResult result = await _letterService.Forward(request, userId);

            if (result.Sucsessed)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [Authorize]
        [HttpPost("reply/{parentLetterId:guid}")]
        public async Task<IActionResult> Reply([FromBody] ReplyDTO reply, Guid parentLetterId)
        {
            bool result;
            string message;
            (result, message) = await _validation.ValidateReplyRequest(reply);
            if (!result)
            {
                return BadRequest(message);
            }

            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _letterService.AddReply(reply, adresseeId, parentLetterId);
            return Ok();
        }
    }
}

