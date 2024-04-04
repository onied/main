using Courses.Models;
using Microsoft.EntityFrameworkCore;
using Task = Courses.Models.Task;

namespace Courses;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Module> Modules { get; set; } = null!;
    public DbSet<Block> Blocks { get; set; } = null!;
    public DbSet<SummaryBlock> SummaryBlocks { get; set; } = null!;
    public DbSet<VideoBlock> VideoBlocks { get; set; } = null!;
    public DbSet<TasksBlock> TasksBlocks { get; set; } = null!;
    public DbSet<Task> Tasks { get; set; } = null!;
    public DbSet<VariantsTask> VariantsTasks { get; set; } = null!;
    public DbSet<InputTask> InputTasks { get; set; } = null!;
    public DbSet<TaskVariant> TaskVariants { get; set; } = null!;
    public DbSet<TaskTextInputAnswer> TaskTextInputAnswers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Block>()
            .HasDiscriminator(block => block.BlockType)
            .HasValue<Block>(BlockType.AnyBlock)
            .HasValue<SummaryBlock>(BlockType.SummaryBlock)
            .HasValue<VideoBlock>(BlockType.VideoBlock)
            .HasValue<TasksBlock>(BlockType.TasksBlock);

        modelBuilder.Entity<User>()
            .HasMany<Course>(u => u.Courses)
            .WithMany();

        modelBuilder.Entity<User>()
            .HasMany<Course>(u => u.ModeratingCourses)
            .WithMany()
            .UsingEntity("course_moderator");

        modelBuilder.Entity<User>()
            .HasMany<Course>(a => a.TeachingCourses)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        var authorId = Guid.Parse("e768e60f-fa76-46d9-a936-4dd5ecbbf326");
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = authorId,
            AvatarHref =
                "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg",
            FirstName = "Василий",
            LastName = "Теркин"
        });
        modelBuilder.Entity<Category>().HasData(new Category
        {
            Id = 1,
            Name = "цифровые технологии"
        });
        modelBuilder.Entity<Course>().HasData(new Course
        {
            Id = 1,
            AuthorId = authorId,
            CategoryId = 1,
            Title = "Название курса. Как я встретил вашу маму. Осуждаю.",
            PictureHref = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Kitten_sleeping.jpg",
            Description =
                "Описание курса. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus. Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.",
            HasCertificates = true,
            HoursCount = 100,
            PriceRubles = 0,
            IsArchived = true,
            IsGlowing = false,
            IsProgramVisible = true
        });
        modelBuilder.Entity<Module>().HasData(new Module
        {
            Id = 1,
            Title = "Такой-то",
            CourseId = 1,
            Index = 0
        }, new Module
        {
            Id = 2,
            Title = "Сякой-то",
            CourseId = 1,
            Index = 1
        });
        modelBuilder.Entity<SummaryBlock>().HasData(new SummaryBlock
        {
            Id = 1,
            Index = 0,
            ModuleId = 1,
            FileHref = "/assets/react.svg",
            FileName = "file_name.svg",
            Title = "Титульник",
            MarkdownText =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi."
        });
        modelBuilder.Entity<VideoBlock>().HasData(new VideoBlock
        {
            Id = 2,
            Index = 1,
            ModuleId = 1,
            Title = "MAKIMA BEAN",
            IsCompleted = true,
            Url = "https://www.youtube.com/watch?v=YfBlwC44gDQ"
        }, new VideoBlock
        {
            Id = 3,
            Index = 2,
            ModuleId = 1,
            Title = "Техас покидает родную гавань",
            Url = "https://vk.com/video-50883936_456243146"
        }, new VideoBlock
        {
            Id = 4,
            Index = 3,
            ModuleId = 1,
            Title = "Александр Асафов о предстоящих президентских выборах",
            Url = "https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd"
        });
        modelBuilder.Entity<TasksBlock>().HasData(new TasksBlock
        {
            Id = 5,
            Index = 4,
            ModuleId = 1,
            Title = "Заголовок блока с заданиями"
        });
        modelBuilder.Entity<VariantsTask>().HasData(new VariantsTask
        {
            Id = 1,
            TasksBlockId = 5,
            MaxPoints = 1,
            TaskType = TaskType.MultipleAnswers,
            Title = "1. Что произошло на пло́щади Тяньаньмэ́нь в 1989 году?"
        }, new VariantsTask
        {
            Id = 2,
            TasksBlockId = 5,
            MaxPoints = 1,
            TaskType = TaskType.SingleAnswer,
            Title = "2. Чипи чипи чапа чапа дуби дуби даба даба?"
        });
        modelBuilder.Entity<InputTask>().HasData(new InputTask
        {
            Id = 3,
            TasksBlockId = 5,
            MaxPoints = 5,
            TaskType = TaskType.InputAnswer,
            Title = "3. Кто?"
        });
        modelBuilder.Entity<Task>().HasData(new Task
        {
            Id = 4,
            TasksBlockId = 5,
            MaxPoints = 1,
            TaskType = TaskType.ManualReview,
            Title = "4. Напишите эссе на тему: “Как я провел лето”"
        });
        modelBuilder.Entity<TaskVariant>().HasData(new TaskVariant
        {
            Id = 1,
            TaskId = 1,
            Description = "Ничего 1",
            IsCorrect = true
        }, new TaskVariant
        {
            Id = 2,
            TaskId = 1,
            Description = "Ничего 2",
            IsCorrect = true
        }, new TaskVariant
        {
            Id = 3,
            TaskId = 1,
            Description = "Ничего 3",
            IsCorrect = true
        }, new TaskVariant
        {
            Id = 4,
            TaskId = 1,
            Description = "Ничего 4",
            IsCorrect = true
        }, new TaskVariant
        {
            Id = 5,
            TaskId = 2,
            Description = "Чипи чипи",
            IsCorrect = true
        }, new TaskVariant
        {
            Id = 6,
            TaskId = 2,
            Description = "Чапа чапа",
            IsCorrect = false
        }, new TaskVariant
        {
            Id = 7,
            TaskId = 2,
            Description = "Дуби дуби",
            IsCorrect = false
        }, new TaskVariant
        {
            Id = 8,
            TaskId = 2,
            Description = "Даба даба",
            IsCorrect = false
        });
        modelBuilder.Entity<TaskTextInputAnswer>().HasData(new TaskTextInputAnswer
        {
            Id = 1,
            TaskId = 3,
            Answer = "Жак Фреско",
            IsCaseSensitive = true
        });
    }
}
