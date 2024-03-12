using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Blocks.Tasks;
using Courses.Dtos.Blocks.Tasks.TaskUserInput;
using Courses.Models;
using Courses.Models.Blocks.Tasks;
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
                    return new VariantsAnswerUserInput()
                    {
                        TaskId = src.TaskId,
                        VariantsIds = src.VariantsIds!
                    };
                case TaskType.InputAnswer:
                    return new InputAnswerUserInput()
                    {
                        TaskId = src.TaskId,
                        Answer = src.Answer!
                    };
                case TaskType.ManualReview:
                    return new ManualReviewUserInput()
                    {
                        TaskId = src.TaskId,
                        Text = src.Text!
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });

        CreateMap<UserInputDto, InputAnswerUserInput>();
        CreateMap<UserInputDto, VariantsAnswerUserInput>();
        CreateMap<UserInputDto, ManualReviewUserInputDto>();

        CreateMap<UserTaskPoints, UserTaskPointsDto>();

        /*
         CreateMap<VariantsAnswerUserInputDto, VariantsAnswerUserInput>();
        CreateMap<InputAnswerUserInputDto, InputAnswerUserInput>();
        CreateMap<ManualReviewUserInputDto, ManualReviewUserInput>();
        */
    }
}