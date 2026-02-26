using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NotesApi.DTOs;

[Authorize]
[ApiController]
[Route("api/notes")]
public class NotesController : ControllerBase
{
    private readonly INoteRepository _noteRepo;

    public NotesController(INoteRepository noteRepo)
    {
        _noteRepo = noteRepo;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var notes = await _noteRepo.GetByOwnerAsync(userId);

        return Ok(notes);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNoteDto dto)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var note = new Note
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            OwnerUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _noteRepo.AddAsync(note);

        return Ok(note);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var note = await _noteRepo.GetByIdAsync(id);
        if (note == null)
            return NotFound();

        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var role = User.FindFirstValue(ClaimTypes.Role);

        if (note.OwnerUserId != userId && role != "Admin")
            return Forbid();

        await _noteRepo.DeleteAsync(note);

        return NoContent();
    }
}