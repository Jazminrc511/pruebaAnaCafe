using Microsoft.EntityFrameworkCore;
public class NotesRepository : INoteRepository
{
    private readonly AppDbContext _context;

    public NotesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Note>> GetByOwnerAsync(Guid ownerUserId) => await
        _context.Notes.Where(n => n.OwnerUserId == ownerUserId).ToListAsync();

    public async Task<Note?> GetByIdAsync(Guid id) => await
        _context.Notes.FindAsync(id);

    public async Task AddAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Note note)
    {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }
}