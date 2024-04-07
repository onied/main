using AutoMapper;
using Courses.Dtos;
using Courses.Dtos.TaskCheckDtos;
using Courses.Models;
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
        CreateMap<Block, BlockDto>().ForMember(dest => dest.Completed,
            expression => expression.MapFrom(block => block.IsCompleted));
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
        CreateMap<TasksBlock, Dtos.TaskCheckDtos.BlockDto>();
        CreateMap<Course, Dtos.TaskCheckDtos.CourseDto>();
        CreateMap<Module, Dtos.TaskCheckDtos.ModuleDto>();
        CreateMap<TaskCheck, TaskCheckDto>();
        CreateMap<Task, Dtos.TaskCheckDtos.TaskDto>()
            .ForMember(dest => dest.Block, opt => opt.MapFrom(src => src.TasksBlock));


        //MassTransit
        CreateMap<UserCreated, User>();
    }
}
