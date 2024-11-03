using Microsoft.EntityFrameworkCore;
using Support.Data.Abstractions;
using Support.Data.Models;

namespace Support.Data.Repositories;

public class MessageRepository(AppDbContext dbContext) : IMessageRepository
{
    public async Task<Message?> GetAsync(Guid messageId)
    {
        return await dbContext.Messages.FindAsync(messageId);
    }

    public async Task<MessageView?> GetViewWithChatAsync(Guid messageId)
    {
        return await dbContext.MessagesView.Include(message => message.Chat)
            .FirstOrDefaultAsync(message => message.Id == messageId);
    }

    public async Task AddAsync(Message message)
    {
        dbContext.Messages.Add(message);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Message message)
    {
        dbContext.Messages.Update(message);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Message?> GetLastMessageInChatAsync(Guid chatId)
    {
        return await dbContext.Messages.Where(message => message.ChatId == chatId)
            .OrderByDescending(message => message.CreatedAt).FirstOrDefaultAsync();
    }
}
