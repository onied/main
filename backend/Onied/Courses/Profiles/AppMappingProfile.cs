using AutoMapper;
using Courses.Dtos;
using Courses.Models;

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
        CreateMap<Category, CategoryDto>();
        CreateMap<Author, AuthorDto>().ForMember(dest => dest.Name,
            expression => expression.MapFrom(
                author => $"{author.FirstName} {author.LastName}"));
        CreateMap<Course, CoursePreviewDto>()
            .ForMember(dest => dest.Price,
            expression => expression.MapFrom(course => course.PriceRubles))
            .ForMember(dest => dest.CourseProgram, 
                expression => expression.MapFrom(
                    course => course.Modules.Select(module => module.Title).ToList()));
    }
}