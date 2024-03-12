using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.Blocks.Tasks;
using Courses.Dtos.Blocks.Tasks.TaskUserInput;
using Courses.Models;
using Courses.Models.Blocks.Tasks;
using Courses.Models.Blocks.Tasks.TaskUserInput;
using Courses.Profiles.Resolvers;
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
        CreateMap<Author, AuthorDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(new AuthorNameResolver()));
        CreateMap<Category, CategoryDto>();
        CreateMap<Course, PreviewDto>().ForMember(preview => preview.CourseAuthor,
                options => options.MapFrom(course => course.Author))
            .ForMember(preview => preview.CourseProgram,
                options => options.MapFrom(new CourseProgramResolver()));
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
                        VariantsIds = src.VariantsIds!,
                        IsDone = src.IsDone
                    };
                case TaskType.InputAnswer:
                    return new InputAnswerUserInput()
                    {
                        TaskId = src.TaskId,
                        Answer = src.Answer!,
                        IsDone = src.IsDone
                    };
                case TaskType.ManualReview:
                    return new ManualReviewUserInput()
                    {
                        TaskId = src.TaskId,
                        Text = src.Text!,
                        IsDone = src.IsDone
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
