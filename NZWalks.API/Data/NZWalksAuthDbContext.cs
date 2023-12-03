using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerId = "ba6fa2da-5994-486c-aa65-257da57ac940";
            var writerId = "fe199885-d8ed-4611-9702-bd48bc4929af";

            var roles = new List<IdentityRole> {
                new IdentityRole{
                Id = readerId,
                ConcurrencyStamp=readerId,
                Name="Reader",
                NormalizedName="Reader".ToUpper(),
                },
                new IdentityRole{
                Id = writerId,
                ConcurrencyStamp=writerId,
                Name="Writer",
                NormalizedName="Writer".ToUpper(),
                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
