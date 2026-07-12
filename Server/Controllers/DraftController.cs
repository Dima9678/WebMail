using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Server.Factories;
using Server.Normaizators;
using Server.Service;
using Server.Validators;
using System.Security.Claims;
using Domain.Models;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DraftController : ControllerBase
    {
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetDrafts()
        {
            return Ok();
        }
        [Authorize]
        [HttpGet("add")]
        public async Task<IActionResult> AddDrafts([FromBody] DraftDTO request)
        {
            return Ok();
        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDraftById(Guid id)
        {
            return Ok();
        }
    }
}
