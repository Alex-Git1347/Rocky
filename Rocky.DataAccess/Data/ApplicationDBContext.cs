using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rocky.Models;
using Rocky.Models.Models;

namespace Rocky.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<InquiryHeader> InquiryHeader { get; set; }
        public DbSet<InquiryDetail> InquiryDetail { get; set; }
    }
}
