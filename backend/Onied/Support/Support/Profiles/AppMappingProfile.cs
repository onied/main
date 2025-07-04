using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Support.Data.Models;
using Support.Dtos.Chat.GetChat.Response;
using Support.Dtos.Hub.Response;
using Support.Dtos.Support.GetChats.Response;
using SupportChatGrpc;

namespace Support.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        // Hub
        CreateMap<MessageView, HubMessageDto>().ForMember(dto => dto.Message,
                options => options
                    .MapFrom(message => message.MessageContent))
            .ForMember(dto => dto.MessageId,
                options => options
                    .MapFrom(message => message.Id))
            .ForMember(dto => dto.SupportNumber,
                options => options
                    .MapFrom(message => message.SupportNumberNullIfUser));
        CreateMap<Message, HubMessageDto>().ForMember(dto => dto.Message,
                options => options
                    .MapFrom(message => message.MessageContent))
            .ForMember(dto => dto.MessageId,
                options => options
                    .MapFrom(message => message.Id))
            .ForMember(dto => dto.SupportNumber,
                options => options
                    .MapFrom(message =>
                        message.UserId == message.Chat.ClientId ? null :
                        message.Chat.Support != null ? (int?)message.Chat.Support.Number : null));
        CreateMap<Data.Models.File, HubMessageFileDto>();

        // GetChat
        CreateMap<Chat, GetChatResponseDto>()
            .ForMember(dest => dest.SupportNumber,
                opt => opt.MapFrom(src => src.Support == null ? (int?)null : src.Support.Number));
        CreateMap<MessageView, GetChatMessageItem>()
            .ForMember(dest => dest.MessageId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Message,
                opt => opt.MapFrom(src => src.MessageContent))
            .ForMember(dest => dest.SupportNumber,
                opt => opt
                    .MapFrom(src => src.SupportNumberNullIfUser));
        CreateMap<Data.Models.File, GetChatMessageFile>();

        // GetChats
        CreateMap<Chat, GetChatsResponseDto>()
            .ForMember(dest => dest.ChatId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LastMessage,
                opt => opt.MapFrom(src => src.Messages.LastOrDefault()));
        CreateMap<MessageView, GetChatsMessageItem>()
            .ForMember(dest => dest.MessageId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Message,
                opt => opt.MapFrom(src => src.MessageContent))
            .ForMember(dest => dest.SupportNumber,
                opt => opt
                    .MapFrom(src => src.SupportNumberNullIfUser));

        CreateMap<HubMessageDto, MessageResponse>()
            .ForMember(dest => dest.CreatedAt, op => op
                .MapFrom(src => src.CreatedAt.ToTimestamp()));
    }
}
