using AutoFixture;
using Courses;
using Courses.Models;
using Task = System.Threading.Tasks.Task;

namespace Tests.Courses.UnitTests.ControllerTests;

public class TestDataGenerator
{
    private readonly AppDbContext _context;
    private readonly IFixture _fixture;

    public TestDataGenerator(AppDbContext context, IFixture fixture)
    {
        _context = context;
        _fixture = fixture;
    }

    public IEnumerable<Course> GenerateTestCourses()
    {
         var sequenceModule = 1;
        var sequenceBlock = 1;
        var sequenceTask = 1;
        
        var courses = _fixture.Build<Course>()
            .With(course => course.Id, 1)
            .CreateMany(1)
            .ToList();
        
        var course = courses.First();
        
        var modules = _fixture.Build<Module>()
                .With(module => module.Id, () => sequenceModule++)
                .With(module => module.Course, course)
                .With(module => module.CourseId, course.Id)
                .CreateMany(1)
                .ToList();
        
        modules.ForEach(module => courses.ForEach(course => course.Modules.Add(module)));

        foreach (var module in modules)
        {
            var summaryBlocks = _fixture.Build<SummaryBlock>()
                .With(block => block.Id, () => sequenceBlock++)
                .With(block => block.Module, module)
                .With(block => block.ModuleId, module.Id)
                .With(block => block.BlockType, BlockType.SummaryBlock)
                .CreateMany(1)
                .ToList();

            var videoBlocks = _fixture.Build<VideoBlock>()
                .With(block => block.Id, () => sequenceBlock++)
                .With(block => block.Module, module)
                .With(block => block.ModuleId, module.Id)
                .With(block => block.BlockType, BlockType.VideoBlock)
                .CreateMany(1)
                .ToList();

            var tasksBlocks = _fixture.Build<TasksBlock>()
                .With(block => block.Id, () => sequenceBlock++)
                .With(block => block.Module, module)
                .With(block => block.ModuleId, module.Id)
                .With(block => block.BlockType, BlockType.TasksBlock)
                .CreateMany(1)
                .ToList();
            
            foreach (var tasksBlock in tasksBlocks)
            {
                var inputTasks = _fixture.Build<InputTask>()
                    .With(task => task.Id, () => sequenceTask++)
                    .With(task => task.TasksBlock, tasksBlock)
                    .With(task => task.TasksBlockId, tasksBlock.Id)
                    .With(task => task.TaskType, TaskType.InputAnswer)
                    .CreateMany(1)
                    .ToList();

                foreach (var inputTask in inputTasks)
                {
                    var answers = _fixture.Build<TaskTextInputAnswer>()
                        .With(answer => answer.Task, inputTask)
                        .With(answer => answer.TaskId, inputTask.Id)
                        .CreateMany(1)
                        .ToList();
                    
                    answers.ForEach(answer => inputTask.Answers.Add(answer));
                }
                
                var variantTasks = _fixture.Build<VariantsTask>()
                    .With(task => task.Id, () => sequenceTask++)
                    .With(task => task.TasksBlock, tasksBlock)
                    .With(task => task.TasksBlockId, tasksBlock.Id)
                    .With(task => task.TaskType, TaskType.MultipleAnswers)
                    .CreateMany(1)
                    .ToList();
                
                foreach (var variantTask in variantTasks)
                {
                    var variants = _fixture.Build<TaskVariant>()
                        .With(variant => variant.Task, variantTask)
                        .With(variant => variant.TaskId, variantTask.Id)
                        .CreateMany(1)
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

        return courses;
    }

    public void AddTestCoursesToDb(IEnumerable<Course> courses)
    {
        _context.Courses.AddRange(courses);

        var modules = courses.SelectMany(course => course.Modules);
        _context.Modules.AddRange(modules);

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