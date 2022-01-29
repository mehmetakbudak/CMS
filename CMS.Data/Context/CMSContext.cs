using CMS.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace CMS.Data.Context
{
    public class CMSContext : DbContext
    {
        public CMSContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }

        public virtual DbSet<Article> Articles { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<AccessRight> AccessRights { get; set; }

        public virtual DbSet<ContactCategory> ContactCategories { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<Todo> Todos { get; set; }

        public virtual DbSet<TodoCategory> TodoCategories { get; set; }

        public virtual DbSet<TodoStatus> TodoStatuses { get; set; }

        public virtual DbSet<UserAccessRight> UserAccessRights { get; set; }

        public virtual DbSet<Menu> Menus { get; set; }

        public virtual DbSet<MenuItems> MenuItems { get; set; }

        public virtual DbSet<Page> Pages { get; set; }

        public virtual DbSet<BlogCategory> BlogCategories { get; set; }

        public virtual DbSet<Blog> Blogs { get; set; }

        public virtual DbSet<SelectedBlogCategory> SelectedBlogCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}