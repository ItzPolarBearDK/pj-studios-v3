using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LobbyController : Controller
    {

        private readonly AppDBContext _context;
        public LobbyController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("Lobbies")]
        public IActionResult GetLobbies()
        {
            var lobbies = _context.Lobbies.ToList();
            return Ok(lobbies);
        }

        [HttpPost("create")]
        public IActionResult CreateLobby([FromBody] CreateLobbyDTO dto)
        {
            var lobby = new Lobby
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Code = GenerateLobbyCode(),
                Users = new List<string>() { dto.Username },
                Status = "Waiting"
            };
            _context.Lobbies.Add(lobby);
            _context.SaveChanges();
            return Ok(lobby);
        }

        private string GenerateLobbyCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost("join")]
        public IActionResult JoinLobby([FromBody] JoinLobbyDTO dto)
        {
            var lobby = _context.Lobbies.FirstOrDefault(l => l.Code == dto.Code);

            if (lobby == null)
            {
                return BadRequest("Lobby not found");
            }

            if (lobby.Users.Contains(dto.Username))
            {
                return BadRequest("Username already taken");
            }

            if (lobby.Users.Count >= 3)
            {
                return BadRequest("Lobby is full");
            }

            lobby.Users.Add(dto.Username);
            _context.SaveChanges();
            return Ok(lobby);
        }

        [HttpDelete("DeleteLobby")]
        public IActionResult DeleteLobby(Guid id)
        {
            var lobby = _context.Lobbies.FirstOrDefault(l => l.Id == id);
            if (lobby == null)
            {
                return NotFound("Lobby not found");
            }
            _context.Lobbies.Remove(lobby);
            _context.SaveChanges();
            return Ok("Lobby deleted");
        }
    }
}
