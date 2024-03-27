using AutoFixture;
using Courses;
using Courses.Models;

namespace Tests.Courses.UnitTests.ControllerTests;

public class TestDataGenerator
{
    private readonly AppDbContext _context;
    private readonly IFixture _fixture;

    public TestDataGenerator(IFixture fixture, AppDbContext? context = null)
    {
        _context = context ?? ContextGenerator.GetContext();
        _fixture = fixture;
    }

    public IEnumerable<Course> GenerateTestCourses()
    {
        var sequenceCourse = 1;
        var sequenceCategory = 1;
        var sequenceModule = 1;
        var sequenceBlock = 1;
        var sequenceTask = 1;
        var isProgramVisible = false;

        var courses = _fixture.Build<Course>()
            .With(course => course.Id, () => sequenceCourse++)
            .With(course => course.IsProgramVisible, () => !isProgramVisible)
            .CreateMany(3)
            .ToList();

        foreach (var course in courses)
        {
            var author = _fixture.Build<User>()
                .With(author1 => author1.Id, Guid.NewGuid)
                .Do(author1 => author1.Courses.Add(course))
                .Create();
            course.Author = author;
            course.AuthorId = author.Id;

            var category = _fixture.Build<Category>()
                .With(category1 => category1.Id, () => sequenceCategory++)
                .Do(category1 => category1.Courses.Add(course))
                .Create();
            course.Category = category;
            course.CategoryId = category.Id;

            var modules = _fixture.Build<Module>()
                .With(module => module.Id, () => sequenceModule++)
                .With(module => module.Course, course)
                .With(module => module.CourseId, course.Id)
                .CreateMany(3)
                .ToList();
            modules.ForEach(module => courses
                .First(course => course.Id == module.CourseId).Modules.Add(module));

            foreach (var module in modules)
            {
                var summaryBlocks = _fixture.Build<SummaryBlock>()
                    .With(block => block.Id, () => sequenceBlock++)
                    .With(block => block.Module, module)
                    .With(block => block.ModuleId, module.Id)
                    .With(block => block.BlockType, BlockType.SummaryBlock)
                    .CreateMany(3)
                    .ToList();

                var videoBlocks = _fixture.Build<VideoBlock>()
                    .With(block => block.Id, () => sequenceBlock++)
                    .With(block => block.Module, module)
                    .With(block => block.ModuleId, module.Id)
                    .With(block => block.BlockType, BlockType.VideoBlock)
                    .CreateMany(3)
                    .ToList();

                var tasksBlocks = _fixture.Build<TasksBlock>()
                    .With(block => block.Id, () => sequenceBlock++)
                    .With(block => block.Module, module)
                    .With(block => block.ModuleId, module.Id)
                    .With(block => block.BlockType, BlockType.TasksBlock)
                    .CreateMany(3)
                    .ToList();

                foreach (var tasksBlock in tasksBlocks)
                {
                    var inputTasks = _fixture.Build<InputTask>()
                        .With(task => task.Id, () => sequenceTask++)
                        .With(task => task.TasksBlock, tasksBlock)
                        .With(task => task.TasksBlockId, tasksBlock.Id)
                        .With(task => task.TaskType, TaskType.InputAnswer)
                        .CreateMany(3)
                        .ToList();

                    foreach (var inputTask in inputTasks)
                    {
                        var answers = _fixture.Build<TaskTextInputAnswer>()
                            .With(answer => answer.Task, inputTask)
                            .With(answer => answer.TaskId, inputTask.Id)
                            .CreateMany(3)
                            .ToList();

                        answers.ForEach(answer => inputTask.Answers.Add(answer));
                    }

                    var variantTasks = _fixture.Build<VariantsTask>()
                        .With(task => task.Id, () => sequenceTask++)
                        .With(task => task.TasksBlock, tasksBlock)
                        .With(task => task.TasksBlockId, tasksBlock.Id)
                        .With(task => task.TaskType, TaskType.MultipleAnswers)
                        .CreateMany(3)
                        .ToList();

                    foreach (var variantTask in variantTasks)
                    {
                        var variants = _fixture.Build<TaskVariant>()
                            .With(variant => variant.Task, variantTask)
                            .With(variant => variant.TaskId, variantTask.Id)
                            .CreateMany(3)
                            .ToList();

                        variants.ForEach(variant => variantTask.Variants.Add(variant));
                    }

                    inputTasks.ForEach(inputTask => tasksBlock.Tasks.Add(inputTask));
                    variantTasks.ForEach(variantTask => tasksBlock.Tasks.Add(variantTask));
                }

                summaryBlocks.ForEach(block => module.Blocks.Add(block));
                videoBlocks.ForEach(block => module.Blocks.Add(block));
                tasksBlocks.ForEach(block => module.Blocks.Add(block));
            }
        }

        return courses;
    }

    public void AddTestCoursesToDb(IEnumerable<Course> courses)
    {
        _context.Courses.AddRange(courses);

        var modules = courses.SelectMany(course => course.Modules);
        _context.Modules.AddRange(modules);

        var authors = courses.Select(course => course.Author);
        _context.Users.AddRange(authors);

        var categories = courses.Select(course => course.Category);
        _context.Categories.AddRange(categories);

        var allBlocks = modules.SelectMany(module => module.Blocks);
        var summaryBlocks = allBlocks.OfType<SummaryBlock>();
        var videoBlocks = allBlocks.OfType<VideoBlock>();
        var tasksBlocks = allBlocks.OfType<TasksBlock>();

        _context.SummaryBlocks.AddRange(summaryBlocks);
        _context.VideoBlocks.AddRange(videoBlocks);
        _context.TasksBlocks.AddRange(tasksBlocks);

        var allTasks = tasksBlocks.SelectMany(block => block.Tasks);
        var inputTasks = allTasks.OfType<InputTask>();
        var variantsTasks = allTasks.OfType<VariantsTask>();

        _context.InputTasks.AddRange(inputTasks);
        _context.VariantsTasks.AddRange(variantsTasks);

        var answers = inputTasks.SelectMany(task => task.Answers);
        var variants = variantsTasks.SelectMany(task => task.Variants);

        _context.TaskTextInputAnswers.AddRange(answers);
        _context.TaskVariants.AddRange(variants);

        _context.SaveChangesAsync();
    }
}
