﻿// <auto-generated />
using System;
using Courses;
using Courses.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Courses.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240309195910_FixedBlockType")]
    partial class FixedBlockType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Courses.Models.Block", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BlockType")
                        .HasColumnType("integer")
                        .HasColumnName("block_type");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_completed");

                    b.Property<int>("ModuleId")
                        .HasColumnType("integer")
                        .HasColumnName("module_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_blocks");

                    b.HasIndex("ModuleId")
                        .HasDatabaseName("ix_blocks_module_id");

                    b.ToTable("blocks", (string)null);

                    b.HasDiscriminator<int>("BlockType").HasValue(0);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Courses.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_courses");

                    b.ToTable("courses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "Название курса. Как я встретил вашу маму. Осуждаю."
                        });
                });

            modelBuilder.Entity("Courses.Models.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("integer")
                        .HasColumnName("course_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_modules");

                    b.HasIndex("CourseId")
                        .HasDatabaseName("ix_modules_course_id");

                    b.ToTable("modules", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CourseId = 1,
                            Title = "Такой-то"
                        },
                        new
                        {
                            Id = 2,
                            CourseId = 1,
                            Title = "Сякой-то"
                        });
                });

            modelBuilder.Entity("Courses.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discriminator");

                    b.Property<int>("MaxPoints")
                        .HasColumnType("integer")
                        .HasColumnName("max_points");

                    b.Property<int?>("Points")
                        .HasColumnType("integer")
                        .HasColumnName("points");

                    b.Property<int>("TaskType")
                        .HasColumnType("integer")
                        .HasColumnName("task_type");

                    b.Property<int>("TasksBlockId")
                        .HasColumnType("integer")
                        .HasColumnName("tasks_block_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(280)
                        .HasColumnType("character varying(280)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_tasks");

                    b.HasIndex("TasksBlockId")
                        .HasDatabaseName("ix_tasks_tasks_block_id");

                    b.ToTable("tasks", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Task");

                    b.UseTphMappingStrategy();

                    b.HasData(
                        new
                        {
                            Id = 4,
                            MaxPoints = 1,
                            TaskType = 3,
                            TasksBlockId = 5,
                            Title = "4. Напишите эссе на тему: “Как я провел лето”"
                        });
                });

            modelBuilder.Entity("Courses.Models.TaskTextInputAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("answer");

                    b.Property<bool>("IsCaseSensitive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_case_sensitive");

                    b.Property<int>("TaskId")
                        .HasColumnType("integer")
                        .HasColumnName("task_id");

                    b.HasKey("Id")
                        .HasName("pk_task_text_input_answers");

                    b.HasIndex("TaskId")
                        .HasDatabaseName("ix_task_text_input_answers_task_id");

                    b.ToTable("task_text_input_answers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Answer = "Жак Фреско",
                            IsCaseSensitive = true,
                            TaskId = 3
                        });
                });

            modelBuilder.Entity("Courses.Models.TaskVariant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(280)
                        .HasColumnType("character varying(280)")
                        .HasColumnName("description");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean")
                        .HasColumnName("is_correct");

                    b.Property<int>("TaskId")
                        .HasColumnType("integer")
                        .HasColumnName("task_id");

                    b.HasKey("Id")
                        .HasName("pk_task_variants");

                    b.HasIndex("TaskId")
                        .HasDatabaseName("ix_task_variants_task_id");

                    b.ToTable("task_variants", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Ничего",
                            IsCorrect = true,
                            TaskId = 1
                        },
                        new
                        {
                            Id = 2,
                            Description = "Ничего",
                            IsCorrect = true,
                            TaskId = 1
                        },
                        new
                        {
                            Id = 3,
                            Description = "Ничего",
                            IsCorrect = true,
                            TaskId = 1
                        },
                        new
                        {
                            Id = 4,
                            Description = "Ничего",
                            IsCorrect = true,
                            TaskId = 1
                        },
                        new
                        {
                            Id = 5,
                            Description = "Чипи чипи",
                            IsCorrect = true,
                            TaskId = 2
                        },
                        new
                        {
                            Id = 6,
                            Description = "Чапа чапа",
                            IsCorrect = false,
                            TaskId = 2
                        },
                        new
                        {
                            Id = 7,
                            Description = "Дуби дуби",
                            IsCorrect = false,
                            TaskId = 2
                        },
                        new
                        {
                            Id = 8,
                            Description = "Даба даба",
                            IsCorrect = false,
                            TaskId = 2
                        });
                });

            modelBuilder.Entity("Courses.Models.SummaryBlock", b =>
                {
                    b.HasBaseType("Courses.Models.Block");

                    b.Property<string>("FileHref")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("file_href");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("file_name");

                    b.Property<string>("MarkdownText")
                        .HasMaxLength(15000)
                        .HasColumnType("character varying(15000)")
                        .HasColumnName("markdown_text");

                    b.HasDiscriminator().HasValue(1);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BlockType = 0,
                            IsCompleted = false,
                            ModuleId = 1,
                            Title = "Титульник",
                            FileHref = "/assets/react.svg",
                            FileName = "file_name.svg",
                            MarkdownText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus.Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. \n Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi."
                        });
                });

            modelBuilder.Entity("Courses.Models.TasksBlock", b =>
                {
                    b.HasBaseType("Courses.Models.Block");

                    b.HasDiscriminator().HasValue(3);

                    b.HasData(
                        new
                        {
                            Id = 5,
                            BlockType = 0,
                            IsCompleted = false,
                            ModuleId = 1,
                            Title = "Заголовок блока с заданиями"
                        });
                });

            modelBuilder.Entity("Courses.Models.VideoBlock", b =>
                {
                    b.HasBaseType("Courses.Models.Block");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("url");

                    b.HasDiscriminator().HasValue(2);

                    b.HasData(
                        new
                        {
                            Id = 2,
                            BlockType = 0,
                            IsCompleted = true,
                            ModuleId = 1,
                            Title = "MAKIMA BEAN",
                            Url = "https://www.youtube.com/watch?v=YfBlwC44gDQ"
                        },
                        new
                        {
                            Id = 3,
                            BlockType = 0,
                            IsCompleted = false,
                            ModuleId = 1,
                            Title = "Техас покидает родную гавань",
                            Url = "https://vk.com/video-50883936_456243146"
                        },
                        new
                        {
                            Id = 4,
                            BlockType = 0,
                            IsCompleted = false,
                            ModuleId = 1,
                            Title = "Александр Асафов о предстоящих президентских выборах",
                            Url = "https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd"
                        });
                });

            modelBuilder.Entity("Courses.Models.InputTask", b =>
                {
                    b.HasBaseType("Courses.Models.Task");

                    b.HasDiscriminator().HasValue("InputTask");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            MaxPoints = 5,
                            Points = 5,
                            TaskType = 2,
                            TasksBlockId = 5,
                            Title = "3. Кто?"
                        });
                });

            modelBuilder.Entity("Courses.Models.VariantsTask", b =>
                {
                    b.HasBaseType("Courses.Models.Task");

                    b.HasDiscriminator().HasValue("VariantsTask");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MaxPoints = 1,
                            TaskType = 1,
                            TasksBlockId = 5,
                            Title = "1. Что произошло на пло́щади Тяньаньмэ́нь в 1989 году?"
                        },
                        new
                        {
                            Id = 2,
                            MaxPoints = 1,
                            Points = 0,
                            TaskType = 0,
                            TasksBlockId = 5,
                            Title = "2. Чипи чипи чапа чапа дуби дуби даба даба?"
                        });
                });

            modelBuilder.Entity("Courses.Models.Block", b =>
                {
                    b.HasOne("Courses.Models.Module", "Module")
                        .WithMany("Blocks")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_blocks_modules_module_id");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("Courses.Models.Module", b =>
                {
                    b.HasOne("Courses.Models.Course", "Course")
                        .WithMany("Modules")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_modules_courses_course_id");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Courses.Models.Task", b =>
                {
                    b.HasOne("Courses.Models.TasksBlock", "TasksBlock")
                        .WithMany("Tasks")
                        .HasForeignKey("TasksBlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tasks_blocks_tasks_block_id");

                    b.Navigation("TasksBlock");
                });

            modelBuilder.Entity("Courses.Models.TaskTextInputAnswer", b =>
                {
                    b.HasOne("Courses.Models.InputTask", "Task")
                        .WithMany("Answers")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_task_text_input_answers_tasks_task_id");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Courses.Models.TaskVariant", b =>
                {
                    b.HasOne("Courses.Models.VariantsTask", "Task")
                        .WithMany("Variants")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_task_variants_tasks_task_id");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Courses.Models.Course", b =>
                {
                    b.Navigation("Modules");
                });

            modelBuilder.Entity("Courses.Models.Module", b =>
                {
                    b.Navigation("Blocks");
                });

            modelBuilder.Entity("Courses.Models.TasksBlock", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Courses.Models.InputTask", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("Courses.Models.VariantsTask", b =>
                {
                    b.Navigation("Variants");
                });
#pragma warning restore 612, 618
        }
    }
}
