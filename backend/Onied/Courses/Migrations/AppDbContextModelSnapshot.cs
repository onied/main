﻿// <auto-generated />
using System;
using Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Courses.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CourseUser", b =>
                {
                    b.Property<int>("CoursesId")
                        .HasColumnType("integer")
                        .HasColumnName("courses_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("CoursesId", "UserId")
                        .HasName("pk_course_user");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_course_user_user_id");

                    b.ToTable("course_user", (string)null);
                });

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

                    b.Property<int>("Index")
                        .HasColumnType("integer")
                        .HasColumnName("index");

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

                    b.HasIndex("ModuleId");

                    b.ToTable("blocks", (string)null);

                    b.HasDiscriminator<int>("BlockType").HasValue(0);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Courses.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "цифровые технологии"
                        });
                });

            modelBuilder.Entity("Courses.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(15000)
                        .HasColumnType("character varying(15000)")
                        .HasColumnName("description");

                    b.Property<bool>("HasCertificates")
                        .HasColumnType("boolean")
                        .HasColumnName("has_certificates");

                    b.Property<int>("HoursCount")
                        .HasColumnType("integer")
                        .HasColumnName("hours_count");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean")
                        .HasColumnName("is_archived");

                    b.Property<bool>("IsGlowing")
                        .HasColumnType("boolean")
                        .HasColumnName("is_glowing");

                    b.Property<bool>("IsProgramVisible")
                        .HasColumnType("boolean")
                        .HasColumnName("is_program_visible");

                    b.Property<string>("PictureHref")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("picture_href");

                    b.Property<int>("PriceRubles")
                        .HasColumnType("integer")
                        .HasColumnName("price_rubles");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_courses");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_courses_author_id");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_courses_category_id");

                    b.ToTable("courses", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"),
                            CategoryId = 1,
                            Description = "Описание курса. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Egestas dui id ornare arcu. Nunc id cursus metus aliquam eleifend mi in nulla posuere. Luctus venenatis lectus magna fringilla urna porttitor. Lobortis elementum nibh tellus molestie. Curabitur gravida arcu ac tortor dignissim convallis aenean. Ut diam quam nulla porttitor massa. Vulputate ut pharetra sit amet aliquam id diam maecenas ultricies. Sagittis id consectetur purus ut faucibus pulvinar elementum integer. Malesuada bibendum arcu vitae elementum curabitur vitae nunc sed velit. Mattis nunc sed blandit libero volutpat sed. Urna neque viverra justo nec. Ullamcorper morbi tincidunt ornare massa. Bibendum est ultricies integer quis auctor elit sed vulputate. Scelerisque eu ultrices vitae auctor eu augue ut lectus. Lacus vel facilisis volutpat est velit. Vitae purus faucibus ornare suspendisse sed nisi lacus. Urna condimentum mattis pellentesque id nibh tortor id. Urna cursus eget nunc scelerisque. Massa id neque aliquam vestibulum morbi. Neque vitae tempus quam pellentesque nec nam aliquam sem et. Mauris pellentesque pulvinar pellentesque habitant morbi. Feugiat in ante metus dictum at. Consequat id porta nibh venenatis cras. Massa massa ultricies mi quis hendrerit dolor. Varius duis at consectetur lorem donec massa sapien faucibus et. Vestibulum sed arcu non odio euismod lacinia at quis risus. Molestie ac feugiat sed lectus vestibulum mattis. In tellus integer feugiat scelerisque varius morbi. Neque ornare aenean euismod elementum. Egestas erat imperdiet sed euismod nisi.",
                            HasCertificates = true,
                            HoursCount = 100,
                            IsArchived = true,
                            IsGlowing = false,
                            IsProgramVisible = true,
                            PictureHref = "https://upload.wikimedia.org/wikipedia/commons/f/fa/Kitten_sleeping.jpg",
                            PriceRubles = 0,
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

                    b.Property<int>("Index")
                        .HasColumnType("integer")
                        .HasColumnName("index");

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
                            Index = 0,
                            Title = "Такой-то"
                        },
                        new
                        {
                            Id = 2,
                            CourseId = 1,
                            Index = 1,
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
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)")
                        .HasColumnName("discriminator");

                    b.Property<int>("MaxPoints")
                        .HasColumnType("integer")
                        .HasColumnName("max_points");

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

                    b.HasIndex("TasksBlockId");

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
                            Description = "Ничего 1",
                            IsCorrect = true,
                            TaskId = 1
                        },
                        new
                        {
                            Id = 2,
                            Description = "Ничего 2",
                            IsCorrect = true,
                            TaskId = 1
                        },
                        new
                        {
                            Id = 3,
                            Description = "Ничего 3",
                            IsCorrect = true,
                            TaskId = 1
                        },
                        new
                        {
                            Id = 4,
                            Description = "Ничего 4",
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

            modelBuilder.Entity("Courses.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AvatarHref")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("avatar_href");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("first_name");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer")
                        .HasColumnName("gender");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_name");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("e768e60f-fa76-46d9-a936-4dd5ecbbf326"),
                            AvatarHref = "https://gas-kvas.com/uploads/posts/2023-02/1676538735_gas-kvas-com-p-vasilii-terkin-detskii-risunok-49.jpg",
                            FirstName = "Василий",
                            LastName = "Теркин"
                        });
                });

            modelBuilder.Entity("course_moderator", b =>
                {
                    b.Property<int>("ModeratingCoursesId")
                        .HasColumnType("integer")
                        .HasColumnName("moderating_courses_id");

                    b.Property<Guid>("User1Id")
                        .HasColumnType("uuid")
                        .HasColumnName("user1id");

                    b.HasKey("ModeratingCoursesId", "User1Id")
                        .HasName("pk_course_moderator");

                    b.HasIndex("User1Id")
                        .HasDatabaseName("ix_course_moderator_user1id");

                    b.ToTable("course_moderator", (string)null);
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

                    b.ToTable("blocks", (string)null);

                    b.HasDiscriminator().HasValue(1);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BlockType = 0,
                            Index = 0,
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

                    b.ToTable("blocks", (string)null);

                    b.HasDiscriminator().HasValue(3);

                    b.HasData(
                        new
                        {
                            Id = 5,
                            BlockType = 0,
                            Index = 4,
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

                    b.ToTable("blocks", (string)null);

                    b.HasDiscriminator().HasValue(2);

                    b.HasData(
                        new
                        {
                            Id = 2,
                            BlockType = 0,
                            Index = 1,
                            IsCompleted = true,
                            ModuleId = 1,
                            Title = "MAKIMA BEAN",
                            Url = "https://www.youtube.com/watch?v=YfBlwC44gDQ"
                        },
                        new
                        {
                            Id = 3,
                            BlockType = 0,
                            Index = 2,
                            IsCompleted = false,
                            ModuleId = 1,
                            Title = "Техас покидает родную гавань",
                            Url = "https://vk.com/video-50883936_456243146"
                        },
                        new
                        {
                            Id = 4,
                            BlockType = 0,
                            Index = 3,
                            IsCompleted = false,
                            ModuleId = 1,
                            Title = "Александр Асафов о предстоящих президентских выборах",
                            Url = "https://rutube.ru/video/1c69be7b3e28cb58368f69473f6c1d96/?r=wd"
                        });
                });

            modelBuilder.Entity("Courses.Models.InputTask", b =>
                {
                    b.HasBaseType("Courses.Models.Task");

                    b.Property<int>("Accuracy")
                        .HasColumnType("integer")
                        .HasColumnName("accuracy");

                    b.Property<bool>("IsCaseSensitive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_case_sensitive");

                    b.Property<bool>("IsNumber")
                        .HasColumnType("boolean")
                        .HasColumnName("is_number");

                    b.ToTable("tasks", (string)null);

                    b.HasDiscriminator().HasValue("InputTask");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            MaxPoints = 5,
                            TaskType = 2,
                            TasksBlockId = 5,
                            Title = "3. Кто?",
                            Accuracy = 0,
                            IsCaseSensitive = true,
                            IsNumber = false
                        });
                });

            modelBuilder.Entity("Courses.Models.VariantsTask", b =>
                {
                    b.HasBaseType("Courses.Models.Task");

                    b.ToTable("tasks", (string)null);

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
                            TaskType = 0,
                            TasksBlockId = 5,
                            Title = "2. Чипи чипи чапа чапа дуби дуби даба даба?"
                        });
                });

            modelBuilder.Entity("CourseUser", b =>
                {
                    b.HasOne("Courses.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_course_user_courses_courses_id");

                    b.HasOne("Courses.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_course_user_users_user_id");
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

            modelBuilder.Entity("Courses.Models.Course", b =>
                {
                    b.HasOne("Courses.Models.User", "Author")
                        .WithMany("TeachingCourses")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_courses_users_author_id");

                    b.HasOne("Courses.Models.Category", "Category")
                        .WithMany("Courses")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_courses_categories_category_id");

                    b.Navigation("Author");

                    b.Navigation("Category");
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

            modelBuilder.Entity("course_moderator", b =>
                {
                    b.HasOne("Courses.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("ModeratingCoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_course_moderator_courses_moderating_courses_id");

                    b.HasOne("Courses.Models.User", null)
                        .WithMany()
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_course_moderator_users_user1id");
                });

            modelBuilder.Entity("Courses.Models.Category", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Courses.Models.Course", b =>
                {
                    b.Navigation("Modules");
                });

            modelBuilder.Entity("Courses.Models.Module", b =>
                {
                    b.Navigation("Blocks");
                });

            modelBuilder.Entity("Courses.Models.User", b =>
                {
                    b.Navigation("TeachingCourses");
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
