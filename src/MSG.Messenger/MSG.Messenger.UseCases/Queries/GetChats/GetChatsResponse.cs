using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Queries.GetChats;

public class GetChatsResponse
{
    public GetChatsResponse(List<ChatModel> chatModels)
    {
        Result = chatModels.Select(x => new Chat(x)).ToList();
    }

    public List<Chat> Result { get; }

    public class Chat
    {
        public Chat(ChatModel chatModel)
        {
            Id = chatModel.Id;
            Name = chatModel.Name;
            CreationDt = chatModel.CreationDt;
            IsDeleted = chatModel.IsDeleted;
            IsDirect = chatModel.IsDirect;
            
            if (chatModel.Messages.Count > 0)
                LastMessage = new LastMessage(chatModel.Messages!);
        }

        /// <summary>
        /// Chat id
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Chat name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Date time of the chat creation 
        /// </summary>
        public DateTime CreationDt { get; }

        /// <summary>
        /// Whether chat is deleted
        /// </summary>
        public bool IsDeleted { get; }

        /// <summary>
        /// Whether chat is direct
        /// </summary>
        public bool IsDirect { get; }

        /// <summary>
        /// Last message
        /// </summary>
        public LastMessage? LastMessage { get; }
    }

    public class LastMessage
    {
        public LastMessage(List<ChatMessageModel> messages)
        {
            var message = messages.MaxBy(x => x.SendingDt)!;
            SentBy = message.SentBy;
            Content = message.Content;
        }

        public Guid SentBy { get; }

        public string Content { get; }
    }
}