using AutoMapper;
using Courses.Data.Models;
using Courses.Dtos.Catalog.Response;
using Courses.Dtos.CheckTasks.Response;
using Courses.Dtos.Course.Response;
using Courses.Dtos.EditCourse.Request;
using Courses.Dtos.ManualReview.Response;
using Courses.Dtos.Moderator.Response;
using Courses.Profiles.Converters;
using Courses.Profiles.Resolvers;
using MassTransit.Data.Messages;
using BlockResponse = Courses.Dtos.Course.Response.BlockResponse;
using CourseResponse = Courses.Dtos.Course.Response.CourseResponse;
using ModuleResponse = Courses.Dtos.Course.Response.ModuleResponse;
using Task = Courses.Data.Models.Task;
using TaskResponse = Courses.Dtos.Course.Response.TaskResponse;

namespace Courses.Profiles;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        AllowNullCollections = true;
        CreateMap<Block, BlockResponse>();
        CreateMap<Course, CourseResponse>();
        CreateMap<Module, ModuleResponse>();
        CreateMap<User, AuthorResponse>().ForMember(dest => dest.Name, opt => opt.MapFrom(new AuthorNameResolver()));
        CreateMap<Category, CategoryResponse>();
        CreateMap<Course, PreviewResponse>().ForMember(preview => preview.CourseAuthor,
                options => options.MapFrom(course => course.Author))
            .ForMember(preview => preview.Price, options => options.MapFrom(course => course.PriceRubles))
            .ForMember(preview => preview.CourseProgram,
                options => options.MapFrom(new CourseProgramResolver()));
        CreateMap<SummaryBlock, SummaryBlockResponse>().ReverseMap();
        CreateMap<VideoBlock, VideoBlockResponse>().ForMember(dest => dest.Href,
            expression => expression.MapFrom(block => block.Url)).ReverseMap();
        CreateMap<TaskVariant, VariantResponse>();
        CreateMap<Task, TaskResponse>().Include<VariantsTask, TaskResponse>();
        CreateMap<VariantsTask, TaskResponse>();
        CreateMap<TasksBlock, TasksBlockResponse>();

        CreateMap<TasksBlock, EditTasksBlockRequest>();
        CreateMap<Task, EditTaskRequest>()
            .Include<VariantsTask, EditTaskRequest>()
            .Include<InputTask, EditTaskRequest>();
        CreateMap<InputTask, EditTaskRequest>();
        CreateMap<VariantsTask, EditTaskRequest>();
        CreateMap<TaskVariant, EditAnswersRequest>();
        CreateMap<TaskTextInputAnswer, EditAnswersRequest>();

        CreateMap<Course, CourseCardResponse>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceRubles));
        CreateMap<UserTaskPoints, UserTaskPointsResponse>();
        CreateMap<EditCourseRequest, Course>()
            .ForMember(dest => dest.PriceRubles, opt => opt.MapFrom(src => src.Price)).ReverseMap();

        CreateMap<CourseResponse, Course>()
            .ForMember(dest => dest.Modules, opt => opt.MapFrom(
                (courseDto, course, i, context) =>
                    context.Mapper.Map<List<Module>>(courseDto.Modules)
            ));
        CreateMap<ModuleResponse, Module>()
            .ForMember(dest => dest.Blocks, opt => opt.MapFrom(
                (moduleDto, module, i, context) =>
                    context.Mapper.Map<List<Block>>(moduleDto.Blocks)
            ));
        CreateMap<BlockResponse, Block>();
        CreateMap<TasksBlock, Dtos.ManualReview.Response.BlockResponse>();
        CreateMap<Course, Dtos.ManualReview.Response.CourseResponse>();
        CreateMap<Module, Dtos.ManualReview.Response.ModuleResponse>();
        CreateMap<Task, Dtos.ManualReview.Response.TaskResponse>()
            .ForMember(dest => dest.Block, opt => opt.MapFrom(src => src.TasksBlock));
        CreateMap<ManualReviewTaskUserAnswer, ManualReviewTaskUserAnswerResponse>();
        CreateMap<ManualReviewTaskUserAnswer, ManualReviewTaskInfoResponse>()
            .ForMember(dest => dest.Index,
                opt => opt.MapFrom(src => src.ManualReviewTaskUserAnswerId))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Task.Title))
            .ForMember(dest => dest.BlockTitle,
                opt => opt.MapFrom(src => src.Task.TasksBlock.Title))
            .ForMember(dest => dest.ModuleTitle,
                opt => opt.MapFrom(src => src.Task.TasksBlock.Module.Title));
        CreateMap<ManualReviewTaskUserAnswer, CourseWithManualReviewTasksResponse>()
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Task.TasksBlock.Module.Course.Title));
        CreateMap<List<ManualReviewTaskUserAnswer>, List<CourseWithManualReviewTasksResponse>>()
            .ConvertUsing<UserAnswerToTasksListConverter>();

        CreateMap<Course, CourseStudentsResponse>()
            .ForMember(dest => dest.Students,
                opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.CourseId,
                opt => opt.MapFrom(src => src.Id));

        CreateMap<User, StudentResponse>()
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
