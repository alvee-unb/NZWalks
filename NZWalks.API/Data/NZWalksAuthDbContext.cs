using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext( DbContextOptions<NZWalksAuthDbContext> options ) : base( options )
        {
        }

        protected override void OnModelCreating( ModelBuilder builder )
        {
            base.OnModelCreating( builder );

            var readerId = "82d7aaa6-7924-4706-b842-285be6a74c42";
            var writerId = "25888972-9779-4539-a5e8-3dc3d65d229e";

            var roles = new List< IdentityRole >{
                new IdentityRole{
                    Id = readerId,
                    ConcurrencyStamp = readerId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                new IdentityRole{
                    Id = writerId,
                    ConcurrencyStamp = writerId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
            };

            builder.Entity< IdentityRole >().HasData( roles );
        }
    }
}
