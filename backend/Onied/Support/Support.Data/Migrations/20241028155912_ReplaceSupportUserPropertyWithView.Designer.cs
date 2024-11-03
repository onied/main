﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Support.Data;

#nullable disable

namespace Support.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241028155912_ReplaceSupportUserPropertyWithView")]
    partial class ReplaceSupportUserPropertyWithView
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Support.Data.Models.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<Guid?>("CurrentSessionId")
                        .HasColumnType("uuid")
                        .HasColumnName("current_session_id");

                    b.Property<Guid?>("SupportId")
                        .HasColumnType("uuid")
                        .HasColumnName("support_id");

                    b.HasKey("Id")
                        .HasName("pk_chats");

                    b.HasIndex("SupportId")
                        .HasDatabaseName("ix_chats_support_id");

                    b.ToTable("chats", (string)null);
                });

            modelBuilder.Entity("Support.Data.Models.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("boolean")
                        .HasColumnName("is_system");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("message_content");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("read_at");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_messages");

                    b.HasIndex("ChatId")
                        .HasDatabaseName("ix_messages_chat_id");

                    b.ToTable("messages", (string)null);
                });

            modelBuilder.Entity("Support.Data.Models.MessageView", b =>
                {
                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("boolean")
                        .HasColumnName("is_system");

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("message_content");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("read_at");

                    b.Property<int?>("SupportNumber")
                        .HasColumnType("integer")
                        .HasColumnName("support_number");

                    b.HasIndex("ChatId")
                        .HasDatabaseName("ix_messages_view_chat_id");

                    b.ToTable((string)null);

                    b.ToView("messages_view", (string)null);
                });

            modelBuilder.Entity("Support.Data.Models.SupportUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("number");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Number"));

                    b.HasKey("Id")
                        .HasName("pk_support_users");

                    b.HasAlternateKey("Number")
                        .HasName("ak_support_users_number");

                    b.ToTable("support_users", (string)null);
                });

            modelBuilder.Entity("Support.Data.Models.Chat", b =>
                {
                    b.HasOne("Support.Data.Models.SupportUser", "Support")
                        .WithMany("ActiveChats")
                        .HasForeignKey("SupportId")
                        .HasConstraintName("fk_chats_support_users_support_id");

                    b.Navigation("Support");
                });

            modelBuilder.Entity("Support.Data.Models.Message", b =>
                {
                    b.HasOne("Support.Data.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_chats_chat_id");

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("Support.Data.Models.MessageView", b =>
                {
                    b.HasOne("Support.Data.Models.Chat", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_messages_view_chats_chat_id");

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("Support.Data.Models.Chat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Support.Data.Models.SupportUser", b =>
                {
                    b.Navigation("ActiveChats");
                });
#pragma warning restore 612, 618
        }
    }
}
