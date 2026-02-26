public interface INoteRepository
{
    Task<List<Note>> GetByOwnerAsync(Guid ownerUserId);
    Task<Note?> GetByIdAsync(Guid id);
    Task AddAsync(Note note);
    Task DeleteAsync(Note note);
}