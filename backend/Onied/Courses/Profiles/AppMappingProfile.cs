using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.ManualReviewDtos.Response;
using Courses.Dtos.ModeratorDtos.Response;
using Courses.Models;
using Courses.Profiles.Converters;
using Courses.Profiles.Resolvers;
using MassTransit.Data.Messages;
using BlockDto = Courses.Dtos.BlockDto;
using CourseDto = Courses.Dtos.CourseDto;
using ModuleDto = Courses.Dtos.ModuleDto;
using Task = Courses.Models.Task;
using TaskDto = Courses.Dtos.TaskDto;

namespace Courses.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        AllowNullCollections = true;
        CreateMap<Block, BlockDto>();
        CreateMap<Course, CourseDto>();
        CreateMap<Module, ModuleDto>();
        CreateMap<User, AuthorDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(new AuthorNameResolver()));
        CreateMap<Category, CategoryDto>();
        CreateMap<Course, PreviewDto>().ForMember(preview => preview.CourseAuthor,
                options => options.MapFrom(course => course.Author))
            .ForMember(preview => preview.Price, options => options.MapFrom(course => course.PriceRubles))
            .ForMember(preview => preview.CourseProgram,
                options => options.MapFrom(new CourseProgramResolver()));
        CreateMap<SummaryBlock, SummaryBlockDto>().ReverseMap();
        CreateMap<VideoBlock, VideoBlockDto>().ForMember(dest => dest.Href,
            expression => expression.MapFrom(block => block.Url)).ReverseMap();
        CreateMap<TaskVariant, VariantDto>();
        CreateMap<Task, TaskDto>().Include<VariantsTask, TaskDto>();
        CreateMap<VariantsTask, TaskDto>();
        CreateMap<TasksBlock, TasksBlockDto>();

        CreateMap<TasksBlock, EditTasksBlockDto>();
        CreateMap<Task, EditTaskDto>()
            .Include<VariantsTask, EditTaskDto>()
            .Include<InputTask, EditTaskDto>();
        CreateMap<InputTask, EditTaskDto>();
        CreateMap<VariantsTask, EditTaskDto>();
        CreateMap<TaskVariant, EditAnswersDto>();
        CreateMap<TaskTextInputAnswer, EditAnswersDto>();

        CreateMap<Course, CourseCardDto>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceRubles));
        CreateMap<UserTaskPoints, UserTaskPointsDto>();
        CreateMap<EditCourseDto, Course>()
            .ForMember(dest => dest.PriceRubles, opt => opt.MapFrom(src => src.Price)).ReverseMap();

        CreateMap<CourseDto, Course>()
            .ForMember(dest => dest.Modules, opt => opt.MapFrom(
                (courseDto, course, i, context) =>
                    context.Mapper.Map<List<Module>>(courseDto.Modules)
            ));
        CreateMap<ModuleDto, Module>()
            .ForMember(dest => dest.Blocks, opt => opt.MapFrom(
                (moduleDto, module, i, context) =>
                    context.Mapper.Map<List<Block>>(moduleDto.Blocks)
            ));
        CreateMap<BlockDto, Block>();
        CreateMap<TasksBlock, Dtos.ManualReviewDtos.Response.BlockDto>();
        CreateMap<Course, Dtos.ManualReviewDtos.Response.CourseDto>();
        CreateMap<Module, Dtos.ManualReviewDtos.Response.ModuleDto>();
        CreateMap<Task, Dtos.ManualReviewDtos.Response.TaskDto>()
            .ForMember(dest => dest.Block, opt => opt.MapFrom(src => src.TasksBlock));
        CreateMap<ManualReviewTaskUserAnswer, ManualReviewTaskUserAnswerDto>();
        CreateMap<ManualReviewTaskUserAnswer, ManualReviewTaskInfoDto>()
            .ForMember(dest => dest.Index,
                opt => opt.MapFrom(src => src.ManualReviewTaskUserAnswerId))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Task.Title))
            .ForMember(dest => dest.BlockTitle,
                opt => opt.MapFrom(src => src.Task.TasksBlock.Title))
            .ForMember(dest => dest.ModuleTitle,
                opt => opt.MapFrom(src => src.Task.TasksBlock.Module.Title));
        CreateMap<ManualReviewTaskUserAnswer, CourseWithManualReviewTasksDto>()
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Task.TasksBlock.Module.Course.Title));
        CreateMap<List<ManualReviewTaskUserAnswer>, List<CourseWithManualReviewTasksDto>>()
            .ConvertUsing<UserAnswerToTasksListConverter>();

        CreateMap<Course, CourseStudentsDto>()
            .ForMember(dest => dest.Students,
                opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.CourseId,
                opt => opt.MapFrom(src => src.Id));

        CreateMap<User, StudentDto>()
            .ForMember(dest => dest.StudentId,
                opt => opt.MapFrom(src => src.Id));

        //MassTransit
        CreateMap<UserCreated, User>();
        CreateMap<CourseCreated, Course>()
            .ForMember(
                dest => dest.PriceRubles,
                opt => opt.MapFrom(src => src.Price))
            .ReverseMap();
        CreateMap<CourseUpdated, Course>()
            .ForMember(
                dest => dest.PriceRubles,
                opt => opt.MapFrom(src => src.Price))
            .ReverseMap();
    }
}
