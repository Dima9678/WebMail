using Domain.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public DraftController(DraftService draftService, ValidationCheck validation)
        {
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
            (result, message) = await _validation.ValidateWriteLetterRequest(request);
            if (!result)
            {
                return BadRequest(message);
            }
            Guid adresseeId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _draftService.Add(request, adresseeId);
            return Ok();
        }

        [Authorize]
        [HttpGet("getbyid/{id:guid}")]
        public async Task<IActionResult> GetDraftById(Guid id)
        {
            DraftDTO draft = await _draftService.GetById(id);
            return Ok(draft);
        }

        [Authorize]
        [HttpPatch("save/{id:guid}")]
        public async Task<IActionResult> Save([FromBody] NewDraftDTO request, Guid id)
        {
            await _draftService.Save(request, id);
            return Ok();
        }
    }
}
