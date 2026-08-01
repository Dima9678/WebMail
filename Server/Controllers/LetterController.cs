using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;
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

        [Authorize]
        [HttpPost("write/reply/{parentLetterId:guid}")]
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
        [HttpGet("getuserletters/{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> GetUserAcceptLetters(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetAcceptLetters(userId, startIndex, endIndex);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpGet("get/send/{startIndex:int}/{endIndex:int}")]
        public async Task<IActionResult> GetUserSentLetters(int startIndex, int endIndex)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetSentLetters(userId, startIndex, endIndex);
            return Ok(userLetters);
        }
        [Authorize]
        [HttpGet("get/send/total")]
        public async Task<IActionResult> GetTotalUserSentLetters()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _letterService.GetTotalSendCount(userId);
            return Ok(total);
        }

        [Authorize]
        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            int total = await _letterService.GetTotalAcceptCount(userId);
            return Ok(total);
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

        [Authorize]
        [HttpPut("changeread/{letterid:guid}")]
        public async Task<IActionResult> ChangeRead(Guid letterid)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _letterService.ChangeIsReaden(letterid, userId);
            return Ok();
        }

        [Authorize]
        [HttpPost("forward")]
        public async Task<IActionResult> ForwardLetter([FromBody]ForwardRequest request)
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
    }
}

