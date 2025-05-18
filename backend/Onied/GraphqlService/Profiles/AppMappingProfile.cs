using AutoMapper;
using Courses.Data.Models;
using GraphqlService.Dtos.Block.Response;
using GraphqlService.Dtos.Course.Response;
using Task = Courses.Data.Models.Task;
using TaskResponse = GraphqlService.Dtos.Block.Response.TaskResponse;

namespace GraphqlService.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        AllowNullCollections = true;
        // TasksBlock
        CreateMap<TaskVariant, VariantResponse>();
        CreateMap<Task, TaskResponse>().Include<VariantsTask, TaskResponse>();
        CreateMap<VariantsTask, TaskResponse>();
        CreateMap<TasksBlock, TasksBlockResponse>();

        // SummaryBlock
        CreateMap<SummaryBlock, SummaryBlockResponse>().ReverseMap();

        // VideoBlock
        CreateMap<VideoBlock, VideoBlockResponse>().ForMember(dest => dest.Href,
            expression => expression.MapFrom(block => block.Url)).ReverseMap();

        // CourseResponse
        CreateMap<Course, CourseResponse>();
    }
}
