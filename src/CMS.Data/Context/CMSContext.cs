using CMS.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace CMS.Data.Context
{
    public class CMSContext : DbContext
    {
        public CMSContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<AccessRight> AccessRights { get; set; }

        public virtual DbSet<AccessRightEndpoint> AccessRightEndpoints { get; set; }

        public virtual DbSet<ContactCategory> ContactCategories { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<TaskDmo> Tasks { get; set; }

        public virtual DbSet<TaskCategory> TaskCategories { get; set; }

        public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

        public virtual DbSet<RoleAccessRight> RoleAccessRights { get; set; }

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

        public virtual DbSet<Tenant> Tenants { get; set; }

        public virtual DbSet<Job> Jobs { get; set; }

        public virtual DbSet<JobLocation> JobLocations { get; set; }

        public virtual DbSet<UserJob> UserJobs { get; set; }

        public virtual DbSet<MenuItemAccessRight> MenuItemAccessRights { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<UserFile> UserFiles { get; set; }

        public virtual DbSet<UserToken> UserTokens { get; set; }

        public virtual DbSet<UserInfo> UserInfos { get; set; }

        public virtual DbSet<Title> Titles { get; set; }

        public virtual DbSet<AccessRightCategory> AccessRightCategories { get; set; }

        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<LanguageCode> LanguageCodes { get; set; }

        public virtual DbSet<LanguageValue> LanguageValues { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}