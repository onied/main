using AutoMapper;
using Support.Data.Models;
using Support.Dtos;

namespace Support.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Message, HubMessageDto>().ForMember(dto => dto.Message,
            options => options
                .MapFrom(message => message.MessageContent))
            .ForMember(dto => dto.SupportNumbers,
            options => options
                .MapFrom((message, _) => message.Chat.Support?.Number))
            .ForMember(dto => dto.MessageId,
            options => options
                .MapFrom(message => message.Id));
    }
}
