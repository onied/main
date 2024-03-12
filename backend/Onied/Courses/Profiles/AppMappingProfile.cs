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
        CreateMap<TaskVariant, VariantDto>().ReverseMap();
        CreateMap<Task, TaskDto>().Include<VariantsTask, TaskDto>();
        CreateMap<VariantsTask, TaskDto>();
        CreateMap<TasksBlock, TasksBlockDto>();

        CreateMap<UserInputDto, UserInput>().ConstructUsing((src, context) =>
        {
            switch (src.TaskType)
            {
                case TaskType.SingleAnswer or TaskType.MultipleAnswers:
                    /*return new VariantsAnswerUserInput()
                    {
                        TaskId = src.TaskId,
                        Variants = context.Mapper.Map<List<TaskVariant>>(src.Variants!)
                    };*/
                    return context.Mapper.Map<VariantsAnswerUserInput>(src);
                case TaskType.InputAnswer:
                    /*return new InputAnswerUserInput()
                    {
                        TaskId = src.TaskId,
                        Answer = src.Answer!
                    };*/
                    return context.Mapper.Map<InputAnswerUserInput>(src);
                case TaskType.ManualReview:
                    /*return new ManualReviewUserInput()
                    {
                        TaskId = src.TaskId,
                        Text = src.Text!
                    };*/
                    return context.Mapper.Map<ManualReviewUserInput>(src);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

        CreateMap<UserInputDto, InputAnswerUserInput>();
        CreateMap<UserInputDto, VariantsAnswerUserInput>();
        CreateMap<UserInputDto, ManualReviewUserInputDto>();

        /*
         CreateMap<VariantsAnswerUserInputDto, VariantsAnswerUserInput>();
        CreateMap<InputAnswerUserInputDto, InputAnswerUserInput>();
        CreateMap<ManualReviewUserInputDto, ManualReviewUserInput>();
        */
    }
}