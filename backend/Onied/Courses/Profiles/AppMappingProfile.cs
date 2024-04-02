using AutoMapper;
using Courses.Dtos;
using Courses.Models;
using Courses.Profiles.Resolvers;
using MassTransit.Data.Messages;
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
        CreateMap<User, AuthorDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(new AuthorNameResolver()));
        CreateMap<Category, CategoryDto>();
        CreateMap<Course, PreviewDto>().ForMember(preview => preview.CourseAuthor,
                options => options.MapFrom(course => course.Author))
            .ForMember(preview => preview.Price, options => options.MapFrom(course => course.PriceRubles))
            .ForMember(preview => preview.CourseProgram,
                options => options.MapFrom(new CourseProgramResolver()));
        CreateMap<SummaryBlock, SummaryBlockDto>();
        CreateMap<VideoBlock, VideoBlockDto>().ForMember(dest => dest.Href,
            expression => expression.MapFrom(block => block.Url));
        CreateMap<TaskVariant, VariantDto>();
        CreateMap<Task, TaskDto>().Include<VariantsTask, TaskDto>();
        CreateMap<VariantsTask, TaskDto>();
        CreateMap<TasksBlock, TasksBlockDto>();
        CreateMap<Course, CourseCardDto>().ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PriceRubles));
        CreateMap<UserTaskPoints, UserTaskPointsDto>();
        CreateMap<EditCourseDto, Course>()
            .ForMember(dest => dest.PriceRubles, opt => opt.MapFrom(src => src.Price));

        //MassTransit
        CreateMap<UserCreated, User>();
    }
}
