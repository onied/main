using Support.Data.Abstractions;

namespace Support.Data.Repositories;

public class ChatRepository(AppDbContext dbContext) : IChatRepository
{

}
