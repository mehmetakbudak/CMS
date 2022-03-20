﻿// <auto-generated />
using System;
using CMS.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Migrations
{
    [DbContext(typeof(CMSContext))]
    [Migration("20220315111934_initial_create")]
    partial class initial_create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CMS.Model.Entity.AccessRight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsShowMenu")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("access_rights");
                });

            modelBuilder.Entity("CMS.Model.Entity.AccessRightEndpoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessRightId")
                        .HasColumnType("int");

                    b.Property<string>("Endpoint")
                        .HasColumnType("text");

                    b.Property<string>("Method")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccessRightId");

                    b.ToTable("access_right_endpoints");
                });

            modelBuilder.Entity("CMS.Model.Entity.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ArticleCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("ArticleCategoryId");

                    b.HasIndex("AuthorId");

                    b.ToTable("articles");
                });

            modelBuilder.Entity("CMS.Model.Entity.ArticleCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("article_categories");
                });

            modelBuilder.Entity("CMS.Model.Entity.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Resume")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("authors");
                });

            modelBuilder.Entity("CMS.Model.Entity.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("NumberOfView")
                        .HasColumnType("int");

                    b.Property<bool>("Published")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("blogs");
                });

            modelBuilder.Entity("CMS.Model.Entity.BlogCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsShowHome")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("blog_categories");
                });

            modelBuilder.Entity("CMS.Model.Entity.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Code")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("chats");
                });

            modelBuilder.Entity("CMS.Model.Entity.ChatMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("chat_messages");
                });

            modelBuilder.Entity("CMS.Model.Entity.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<int>("SourceType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("CMS.Model.Entity.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ContactCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContactCategoryId");

                    b.ToTable("contacts");
                });

            modelBuilder.Entity("CMS.Model.Entity.ContactCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("contact_categories");
                });

            modelBuilder.Entity("CMS.Model.Entity.MailTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("TemplateType")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("mail_templates");
                });

            modelBuilder.Entity("CMS.Model.Entity.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeletable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("menus");
                });

            modelBuilder.Entity("CMS.Model.Entity.MenuItems", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MenuId")
                        .HasColumnType("int");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("menu_items");
                });

            modelBuilder.Entity("CMS.Model.Entity.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<bool>("Published")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("pages");
                });

            modelBuilder.Entity("CMS.Model.Entity.SelectedBlogCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BlogCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("BlogId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlogCategoryId");

                    b.HasIndex("BlogId");

                    b.ToTable("selected_blog_categories");
                });

            modelBuilder.Entity("CMS.Model.Entity.Todo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AssignUserId")
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("TodoCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("TodoStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("AssignUserId");

                    b.HasIndex("TodoCategoryId");

                    b.HasIndex("TodoStatusId");

                    b.ToTable("todos");
                });

            modelBuilder.Entity("CMS.Model.Entity.TodoCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("todo_categories");
                });

            modelBuilder.Entity("CMS.Model.Entity.TodoStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("TodoCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TodoCategoryId");

                    b.ToTable("todo_statuses");
                });

            modelBuilder.Entity("CMS.Model.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("text");

                    b.Property<string>("HashCode")
                        .HasColumnType("text");

                    b.Property<DateTime>("InsertedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PasswordExpireDate")
                        .HasColumnType("datetime");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TokenExpireDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("CMS.Model.Entity.UserAccessRight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessRightId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccessRightId");

                    b.HasIndex("UserId");

                    b.ToTable("user_access_rights");
                });

            modelBuilder.Entity("CMS.Model.Entity.WebsiteParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<bool>("Required")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<bool>("Visible")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("web_site_parameters");
                });

            modelBuilder.Entity("CMS.Model.Entity.AccessRightEndpoint", b =>
                {
                    b.HasOne("CMS.Model.Entity.AccessRight", "AccessRight")
                        .WithMany("AccessRightEndpoints")
                        .HasForeignKey("AccessRightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.Article", b =>
                {
                    b.HasOne("CMS.Model.Entity.ArticleCategory", "ArticleCategory")
                        .WithMany("Article")
                        .HasForeignKey("ArticleCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMS.Model.Entity.Author", "Author")
                        .WithMany("Article")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.ChatMessage", b =>
                {
                    b.HasOne("CMS.Model.Entity.Chat", "Chat")
                        .WithMany("ChatMessages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMS.Model.Entity.User", "User")
                        .WithMany("ChatMessages")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CMS.Model.Entity.Comment", b =>
                {
                    b.HasOne("CMS.Model.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.Contact", b =>
                {
                    b.HasOne("CMS.Model.Entity.ContactCategory", "ContactCategory")
                        .WithMany("Contacts")
                        .HasForeignKey("ContactCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.MenuItems", b =>
                {
                    b.HasOne("CMS.Model.Entity.Menu", "Menu")
                        .WithMany("MenuItems")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.SelectedBlogCategory", b =>
                {
                    b.HasOne("CMS.Model.Entity.BlogCategory", "BlogCategory")
                        .WithMany("SelectedBlogCategories")
                        .HasForeignKey("BlogCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMS.Model.Entity.Blog", "Blog")
                        .WithMany("SelectedBlogCategories")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.Todo", b =>
                {
                    b.HasOne("CMS.Model.Entity.User", "AssignUser")
                        .WithMany()
                        .HasForeignKey("AssignUserId");

                    b.HasOne("CMS.Model.Entity.TodoCategory", "TodoCategory")
                        .WithMany("Todos")
                        .HasForeignKey("TodoCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMS.Model.Entity.TodoStatus", "TodoStatus")
                        .WithMany("Todos")
                        .HasForeignKey("TodoStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.TodoStatus", b =>
                {
                    b.HasOne("CMS.Model.Entity.TodoCategory", "TodoCategory")
                        .WithMany()
                        .HasForeignKey("TodoCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CMS.Model.Entity.UserAccessRight", b =>
                {
                    b.HasOne("CMS.Model.Entity.AccessRight", "AccessRight")
                        .WithMany("UserAccessRights")
                        .HasForeignKey("AccessRightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMS.Model.Entity.User", "User")
                        .WithMany("UserAccessRights")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
