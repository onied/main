using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Blocks.Tasks.TaskUserInput;
using Courses.Models;
using Courses.Models.Blocks.Tasks.TaskUserInput;
using Task = Courses.Models.Task;

namespace Courses.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<Block, BlockDto>().ForMember(dest => dest.Completed,
            expression => expression.MapFrom(block => block.IsCompleted));
        CreateMap<Course, CourseDto>();
        CreateMap<Module, ModuleDto>();
        CreateMap<SummaryBlock, SummaryBlockDto>();
        CreateMap<VideoBlock, VideoBlockDto>().ForMember(dest => dest.Href,
            expression => expression.MapFrom(block => block.Url));
        CreateMap<TaskVariant, VariantDto>();
        CreateMap<Task, TaskDto>().Include<VariantsTask, TaskDto>();
        CreateMap<VariantsTask, TaskDto>();
        CreateMap<TasksBlock, TasksBlockDto>();
        
        // UserInput
        CreateMap<UserInput, UserInputDto>().ReverseMap(); // TODO: обработать инициализацию Task для ReverseMap
        CreateMap<InputAnswerUserInput, InputAnswerUserInputDto>().ReverseMap();
        CreateMap<VariantsAnswerUserInput, VariantsAnswerUserInputDto>().ReverseMap();
        CreateMap<ManualReviewUserInput, ManualReviewUserInputDto>();
    }
}