using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;
using Domain.Models;
using Server.Validators;
using Domain.Models.DTO;
using Server.Service;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LetterController : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly ValidationCheck _validation;

        private LetterService _letterService;
        
        public LetterController(DatabaseContext db, ValidationCheck validation)
        {
            _db = db;
            _validation = validation;
        }

        [Authorize]
        [HttpPost("write")]
        public async Task<IActionResult> Write([FromBody] NewLetterDTO request)
        {
            bool result;
            string message;
            (result, message) = await _validation.ValidateWriteLetterRequest(request);
            string? adresseeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _letterService.Add(request, adresseeId);
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
           LetterDTO letterDTO = await _letterService.GetById(id);

            return Ok(letterDTO);
        }

        [Authorize]
        [HttpGet("getuserletters")]
        public async Task<IActionResult> GetUserLetters()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetLetters(userId);
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
        [HttpGet("getusersentletters")]
        public async Task<IActionResult> GetUserSentLetters()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<LetterDTO> userLetters = await _letterService.GetSentLetters(userId);
            return Ok(userLetters);
        }

        [Authorize]
        [HttpPut("changestarred/{id:guid}")]
        public async Task<IActionResult> ChangeStarred(Guid id)
        {
            _letterService.ChangeStarred(id);
            return Ok();
        }

    }
}

    