﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Purchases.Data;

#nullable disable

namespace Purchases.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240415082438_addedTitleForSubscription")]
    partial class addedTitleForSubscription
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Purchases.Data.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<bool>("HasCertificates")
                        .HasColumnType("boolean")
                        .HasColumnName("has_certificates");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_courses");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_courses_author_id");

                    b.ToTable("courses", (string)null);
                });

            modelBuilder.Entity("Purchases.Data.Models.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_purchases");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_purchases_user_id");

                    b.ToTable("purchases", (string)null);
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.PurchaseDetails", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<int>("PurchaseType")
                        .HasColumnType("integer")
                        .HasColumnName("purchase_type");

                    b.HasKey("Id")
                        .HasName("pk_purchase_details");

                    b.ToTable("purchase_details", (string)null);

                    b.HasDiscriminator<int>("PurchaseType").HasValue(0);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Purchases.Data.Models.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ActiveCoursesNumber")
                        .HasColumnType("integer")
                        .HasColumnName("active_courses_number");

                    b.Property<bool>("AdsEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("ads_enabled");

                    b.Property<bool>("CertificatesEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("certificates_enabled");

                    b.Property<bool>("CoursesHighlightingEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("courses_highlighting_enabled");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_subscriptions");

                    b.ToTable("subscriptions", (string)null);
                });

            modelBuilder.Entity("Purchases.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("integer")
                        .HasColumnName("subscription_id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("SubscriptionId")
                        .HasDatabaseName("ix_users_subscription_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.CertificatePurchaseDetails", b =>
                {
                    b.HasBaseType("Purchases.Data.Models.PurchaseDetails.PurchaseDetails");

                    b.Property<int>("CourseId")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("integer")
                        .HasColumnName("CourseId");

                    b.HasIndex("CourseId")
                        .HasDatabaseName("ix_purchase_details_course_id");

                    b.ToTable("purchase_details", (string)null);

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.CoursePurchaseDetails", b =>
                {
                    b.HasBaseType("Purchases.Data.Models.PurchaseDetails.PurchaseDetails");

                    b.Property<int>("CourseId")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("integer")
                        .HasColumnName("CourseId");

                    b.HasIndex("CourseId")
                        .HasDatabaseName("ix_purchase_details_course_id");

                    b.ToTable("purchase_details", (string)null);

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.SubscriptionPurchaseDetails", b =>
                {
                    b.HasBaseType("Purchases.Data.Models.PurchaseDetails.PurchaseDetails");

                    b.Property<bool>("AutoRenewalEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("auto_renewal_enabled");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_date");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("integer")
                        .HasColumnName("subscription_id");

                    b.HasIndex("SubscriptionId")
                        .HasDatabaseName("ix_purchase_details_subscription_id");

                    b.ToTable("purchase_details", (string)null);

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Purchases.Data.Models.Course", b =>
                {
                    b.HasOne("Purchases.Data.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_courses_users_author_id");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Purchases.Data.Models.Purchase", b =>
                {
                    b.HasOne("Purchases.Data.Models.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_purchases_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.PurchaseDetails", b =>
                {
                    b.HasOne("Purchases.Data.Models.Purchase", null)
                        .WithOne("PurchaseDetails")
                        .HasForeignKey("Purchases.Data.Models.PurchaseDetails.PurchaseDetails", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_purchase_details_purchases_id");
                });

            modelBuilder.Entity("Purchases.Data.Models.User", b =>
                {
                    b.HasOne("Purchases.Data.Models.Subscription", "Subscription")
                        .WithMany("Users")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_users_subscriptions_subscription_id");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.CertificatePurchaseDetails", b =>
                {
                    b.HasOne("Purchases.Data.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_purchase_details_courses_course_id");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.CoursePurchaseDetails", b =>
                {
                    b.HasOne("Purchases.Data.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_purchase_details_courses_course_id");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Purchases.Data.Models.PurchaseDetails.SubscriptionPurchaseDetails", b =>
                {
                    b.HasOne("Purchases.Data.Models.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_purchase_details_subscriptions_subscription_id");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("Purchases.Data.Models.Purchase", b =>
                {
                    b.Navigation("PurchaseDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("Purchases.Data.Models.Subscription", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Purchases.Data.Models.User", b =>
                {
                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
