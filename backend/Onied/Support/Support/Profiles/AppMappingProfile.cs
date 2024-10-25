using AutoMapper;
using Support.Data.Models;
using Support.Dtos.Chat.GetChat.Response;
using Support.Dtos.Support.GetChats.Response;

namespace Support.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        // GetChat
        CreateMap<Chat, GetChatResponseDto>()
            .ForMember(dest => dest.SupportNumber,
                opt => opt.MapFrom(src => src.Support == null ? (int?)null : src.Support.Number));
        CreateMap<Message, GetChatMessageItem>()
            .ForMember(dest => dest.SupportNumber,
                opt => opt.MapFrom(src => src.Chat.Support == null ? (int?)null : src.Chat.Support.Number))
            .ForMember(dest => dest.MessageId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Message,
                opt => opt.MapFrom(src => src.MessageContent));

        // GetChats
        CreateMap<Chat, GetChatsResponseDto>()
            .ForMember(dest => dest.ChatId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LastMessage,
                opt => opt.MapFrom(src => src.Messages.OrderBy(m => m.CreatedAt).LastOrDefault()));
        CreateMap<Message, GetChatsMessageItem>()
            .ForMember(dest => dest.SupportNumber,
                opt => opt.MapFrom(src => src.Chat.Support == null ? (int?)null : src.Chat.Support.Number))
            .ForMember(dest => dest.MessageId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Message,
                opt => opt.MapFrom(src => src.MessageContent));
    }
}
