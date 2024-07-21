﻿using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

public interface IChatRepository
{
    public Task<ChatModel> CreateAsync(ChatModel chat);
    
    public Task<ChatModel> GetOrCreateDirectAsync(Guid senderId, Guid receiverId);

    public Task<ChatModel?> GetAsync(Guid chatId, bool includeMembers = false, bool? isDirect = null);

    public Task MarkAsDeletedAsync(ChatModel chat, bool deleteMembers = false);
}