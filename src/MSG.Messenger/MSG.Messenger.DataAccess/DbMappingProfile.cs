using AutoMapper;
using MSG.Messenger.Core;
using MSG.Messenger.DataAccess.Entities;

namespace MSG.Messenger.DataAccess;

public class DbMappingProfile : Profile
{
    public DbMappingProfile()
    {
        CreateMap<Chat, ChatModel>();

        CreateMap<ChatModel, Chat>()
            .ForMember(x => x.Members, x => x.Ignore())
            .AfterMap((x, y) => y.Members = x.Members.Select(member => new ChatMember
            {
                ChatId = x.Id,
                UserId = member.UserId,
                IsAdmin = member.IsAdmin
            }).ToList());

        CreateMap<ChatMember, ChatMemberModel>().ReverseMap();

        CreateMap<Message, MessageModel>().ReverseMap();
    }
}