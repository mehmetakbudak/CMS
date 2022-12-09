using CMS.Storage.Entity;
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

        public virtual DbSet<AccessRightEndpoint> AccessRightEndpoints { get; set; }

        public virtual DbSet<ContactCategory> ContactCategories { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<TaskDmo> Todos { get; set; }

        public virtual DbSet<TodoCategory> TodoCategories { get; set; }

        public virtual DbSet<TodoStatus> TodoStatuses { get; set; }

        public virtual DbSet<UserAccessRight> UserAccessRights { get; set; }

        public virtual DbSet<Menu> Menus { get; set; }

        public virtual DbSet<MenuItem> MenuItems { get; set; }

        public virtual DbSet<Page> Pages { get; set; }

        public virtual DbSet<BlogCategory> BlogCategories { get; set; }

        public virtual DbSet<Blog> Blogs { get; set; }

        public virtual DbSet<SelectedBlogCategory> SelectedBlogCategories { get; set; }

        public virtual DbSet<WebsiteParameter> WebsiteParameters { get; set; }

        public virtual DbSet<MailTemplate> MailTemplates { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<Service> Services { get; set; }

        public virtual DbSet<Testimonial> Testimonials { get; set; }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<HomepageSlider> HomepageSliders { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<SourceTag> SourceTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}